using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TechExpress.Repository;
using TechExpress.Repository.Repositories;

namespace TechExpress.Service.Services;

public record MonthlyRevenueServiceItem(
    string Month,
    decimal Revenue);

public record ProductSalesServiceItem(
    Guid ProductId,
    string ProductName,
    int TotalQuantitySold,
    decimal Revenue,
    decimal ContributionRatio);

public record MonthlyRevenueReportServiceResult(
    DateTimeOffset StartMonth,
    DateTimeOffset EndMonth,
    List<MonthlyRevenueServiceItem> Items);

public record BestWorstSalesServiceResult(
    decimal TotalRevenue,
    List<ProductSalesServiceItem> BestSelling,
    List<ProductSalesServiceItem> LeastSelling);

public record RevenueForecastServiceItem(
    string Month,
    decimal PredictedRevenue,
    string Reason);

public record ProductImportSuggestionServiceItem(
    Guid ProductId,
    string ProductName,
    int SuggestedQuantity,
    string Reason);

public record DashboardAiInsightsServiceResult(
    bool AiGenerated,
    string Analysis,
    List<RevenueForecastServiceItem> Forecast,
    List<ProductImportSuggestionServiceItem> ProductImportSuggestions,
    List<string> SuggestedActions);

public class DashboardService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public DashboardService(
        UnitOfWork unitOfWork,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<MonthlyRevenueReportServiceResult> HandleGetMonthlyRevenueAsync(
        Guid? brandId,
        Guid? categoryId,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        CancellationToken ct)
    {
        var (startMonth, endMonth) = ResolveMonthRange(fromDate, toDate);

        var grouped = await _unitOfWork.DashboardRepository.GetMonthlyRevenueByMonthAsync(
            brandId,
            categoryId,
            startMonth,
            endMonth,
            ct);

        int monthsCount = ((endMonth.Year - startMonth.Year) * 12) + (endMonth.Month - startMonth.Month) + 1;
        var monthRevenueMap = grouped.ToDictionary(k => (k.Year * 12 + k.Month), v => v.Revenue);

        var items = new List<MonthlyRevenueServiceItem>(monthsCount);
        for (int i = 0; i < monthsCount; i++)
        {
            var cursor = startMonth.AddMonths(i);
            var key = cursor.Year * 12 + cursor.Month;
            monthRevenueMap.TryGetValue(key, out var revenue);

            items.Add(new MonthlyRevenueServiceItem($"{cursor.Year:D4}-{cursor.Month:D2}", revenue));
        }

        return new MonthlyRevenueReportServiceResult(startMonth, endMonth, items);
    }

    public async Task<string> HandleBuildMonthlyRevenueFilterTextAsync(Guid? brandId, Guid? categoryId, CancellationToken ct)
    {
        if (!brandId.HasValue && !categoryId.HasValue)
            return "Filter: Tất cả (Không giới hạn Brand/Category)";

        string? brandName = null;
        string? categoryName = null;

        if (brandId.HasValue)
            brandName = await _unitOfWork.DashboardRepository.GetBrandNameAsync(brandId.Value, ct);

        if (categoryId.HasValue)
            categoryName = await _unitOfWork.DashboardRepository.GetCategoryNameAsync(categoryId.Value, ct);

        string brandText = brandId.HasValue
            ? $"Brand = {(!string.IsNullOrWhiteSpace(brandName) ? brandName : brandId.ToString())}"
            : string.Empty;

        string categoryText = categoryId.HasValue
            ? $"Category = {(!string.IsNullOrWhiteSpace(categoryName) ? categoryName : categoryId.ToString())}"
            : string.Empty;

        string joined = string.Join(" | ", new[] { brandText, categoryText }.Where(s => !string.IsNullOrWhiteSpace(s)));
        return $"Filter: {joined}";
    }

    public async Task<BestWorstSalesServiceResult> HandleGetBestWorstSellingProductsAsync(
        int top,
        Guid? brandId,
        Guid? categoryId,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        CancellationToken ct)
    {
        var (startMonth, endMonth) = ResolveMonthRange(fromDate, toDate);

        var productStats = await _unitOfWork.DashboardRepository.GetProductRevenueStatsAsync(
            brandId,
            categoryId,
            startMonth,
            endMonth,
            ct);

        var totalRevenue = productStats.Sum(x => x.Revenue);

        List<ProductSalesServiceItem> BuildList(bool isBest)
        {
            var ordered = isBest
                ? productStats.OrderByDescending(x => x.Revenue)
                : productStats.OrderBy(x => x.Revenue);

            return ordered
                .Take(top)
                .Select(x => new ProductSalesServiceItem(
                    x.ProductId,
                    x.ProductName,
                    x.TotalQuantitySold,
                    x.Revenue,
                    totalRevenue > 0 ? (x.Revenue / totalRevenue) : 0m))
                .ToList();
        }

        return new BestWorstSalesServiceResult(
            totalRevenue,
            BuildList(true),
            BuildList(false));
    }

    public async Task<DashboardAiInsightsServiceResult> HandleGetAiRevenueInsightsAsync(
        Guid? brandId,
        Guid? categoryId,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        int forecastMonths,
        int topProducts,
        CancellationToken ct)
    {
        var revenueReport = await HandleGetMonthlyRevenueAsync(brandId, categoryId, fromDate, toDate, ct);

        int monthsCount = revenueReport.Items.Count;
        var productStats = await _unitOfWork.DashboardRepository.GetProductRevenueStatsAsync(
            brandId,
            categoryId,
            revenueReport.StartMonth,
            revenueReport.EndMonth,
            ct);

        var productContext = productStats
            .OrderByDescending(x => x.Revenue)
            .ThenByDescending(x => x.TotalQuantitySold)
            .Take(20)
            .ToList();

        var fallbackForecast = BuildFallbackForecast(revenueReport.Items, revenueReport.EndMonth, forecastMonths);
        var fallbackSuggestions = BuildFallbackSuggestions(productContext, monthsCount, topProducts);

        var prompt = BuildDashboardAiPrompt(
            revenueReport.Items,
            productContext,
            forecastMonths,
            topProducts,
            revenueReport.StartMonth,
            revenueReport.EndMonth);

        var modelName = _configuration["AI:Ollama:Model"] ?? "qwen2.5:7b";

        try
        {
            var rawResponse = await CallOllamaGenerateAsync(prompt, ct);
            var parsed = ParseDashboardAiOutput(rawResponse);

            if (parsed is null)
            {
                return new DashboardAiInsightsServiceResult(
                    false,
                    rawResponse,
                    fallbackForecast,
                    fallbackSuggestions,
                    []);
            }

            var aiForecast = parsed.Forecast?
                .Where(x => !string.IsNullOrWhiteSpace(x.Month))
                .Take(forecastMonths)
                .Select(x => new RevenueForecastServiceItem(
                    x.Month!,
                    x.PredictedRevenue,
                    string.IsNullOrWhiteSpace(x.Reason) ? "Dự báo từ mô hình AI" : x.Reason))
                .ToList() ?? [];

            var aiSuggestions = parsed.ImportSuggestions?
                .Take(topProducts)
                .Select(x =>
                {
                    var resolvedProductId = ResolveProductId(x.ProductId, x.ProductName, productContext);
                    if (!resolvedProductId.HasValue || string.IsNullOrWhiteSpace(x.ProductName))
                        return null;

                    return new ProductImportSuggestionServiceItem(
                        resolvedProductId.Value,
                        x.ProductName,
                        x.SuggestedQuantity > 0 ? x.SuggestedQuantity : 1,
                        string.IsNullOrWhiteSpace(x.Reason) ? "Đề xuất từ mô hình AI" : x.Reason);
                })
                .Where(x => x is not null)
                .Select(x => x!)
                .ToList() ?? [];

            return new DashboardAiInsightsServiceResult(
                true,
                $"[{modelName}] {parsed.Overview ?? "Phân tích doanh thu tổng quan."}",
                aiForecast.Count > 0 ? aiForecast : fallbackForecast,
                aiSuggestions.Count > 0 ? aiSuggestions : fallbackSuggestions,
                parsed.Actions?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? []);
        }
        catch
        {
            return new DashboardAiInsightsServiceResult(
                false,
                "Không kết nối được với A.I , đã trả kết quả dự báo theo dữ liệu lịch sử nội bộ.",
                fallbackForecast,
                fallbackSuggestions,
                []);
        }
    }

    private static (DateTimeOffset StartMonth, DateTimeOffset EndMonth) ResolveMonthRange(DateTimeOffset? fromDate, DateTimeOffset? toDate)
    {
        var end = toDate ?? DateTimeOffset.Now;
        var start = fromDate ?? end.AddMonths(-11);

        var startMonth = new DateTimeOffset(start.Year, start.Month, 1, 0, 0, 0, start.Offset);
        var endMonth = new DateTimeOffset(end.Year, end.Month, 1, 0, 0, 0, end.Offset);

        return (startMonth, endMonth);
    }

    private string BuildDashboardAiPrompt(
        List<MonthlyRevenueServiceItem> monthlyRevenue,
        List<ProductRevenueData> productStats,
        int forecastMonths,
        int topProducts,
        DateTimeOffset startMonth,
        DateTimeOffset endMonth)
    {
        var monthlyRevenueJson = JsonSerializer.Serialize(monthlyRevenue);
        var productStatsJson = JsonSerializer.Serialize(productStats);

        return $$"""
Bạn là chuyên gia phân tích doanh thu cho cửa hàng linh kiện máy tính TechExpress.

Phạm vi dữ liệu: từ {{startMonth:yyyy-MM}} đến {{endMonth:yyyy-MM}}.
Dữ liệu doanh thu theo tháng (JSON):
{{monthlyRevenueJson}}

Dữ liệu sản phẩm theo doanh thu (JSON):
{{productStatsJson}}

Hãy phân tích và trả về CHÍNH XÁC một JSON object duy nhất, không dùng markdown, không thêm văn bản bên ngoài JSON.

Schema bắt buộc:
{
  "overview": "string",
  "forecast": [
    {
      "month": "yyyy-MM",
      "predictedRevenue": 0,
      "reason": "string"
    }
  ],
  "importSuggestions": [
    {
      "productId": "guid",
      "productName": "string",
      "suggestedQuantity": 0,
      "reason": "string"
    }
  ],
  "actions": ["string"]
}

Yêu cầu:
0) Toàn bộ nội dung text trong overview/reason/actions PHẢI là tiếng Việt, không dùng ngôn ngữ khác.
1) Dự báo đúng {{forecastMonths}} tháng kế tiếp sau tháng mới nhất trong dữ liệu.
2) Chỉ đề xuất tối đa {{topProducts}} sản phẩm cho kế hoạch nhập hàng để doanh thu ổn định.
3) Ưu tiên sản phẩm có doanh thu tốt và số lượng bán đều.
4) suggestedQuantity phải là số nguyên dương.
5) productId phải là UUID hợp lệ và ưu tiên lấy từ danh sách dữ liệu sản phẩm đã cung cấp.
""";
    }

    private static Guid? ResolveProductId(string? rawProductId, string? productName, List<ProductRevenueData> productContext)
    {
        var validProductIds = productContext.Select(x => x.ProductId).ToHashSet();

        if (!string.IsNullOrWhiteSpace(rawProductId)
            && Guid.TryParse(rawProductId, out var parsedId)
            && validProductIds.Contains(parsedId))
        {
            return parsedId;
        }

        if (string.IsNullOrWhiteSpace(productName))
            return null;

        var matched = productContext.FirstOrDefault(x =>
            string.Equals(x.ProductName, productName, StringComparison.OrdinalIgnoreCase));

        return matched?.ProductId;
    }

    private async Task<string> CallOllamaGenerateAsync(string prompt, CancellationToken ct)
    {
        var baseUrl = _configuration["AI:Ollama:BaseUrl"] ?? "http://localhost:11434";
        var modelName = _configuration["AI:Ollama:Model"] ?? "qwen2.5:7b";
        var timeoutSeconds = int.TryParse(_configuration["AI:Ollama:TimeoutSeconds"], out var parsed)
            ? Math.Clamp(parsed, 10, 300)
            : 120;

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/generate")
        {
            Content = JsonContent.Create(new
            {
                model = modelName,
                prompt,
                stream = false,
                options = new
                {
                    temperature = 0.2,
                    top_p = 0.9,
                    num_predict = 1200
                }
            })
        };

        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>(cancellationToken: ct);
        if (payload is null || string.IsNullOrWhiteSpace(payload.Response))
            throw new InvalidOperationException("A.I không trả về dữ liệu hợp lệ.");

        return payload.Response;
    }

    private static DashboardAiOutput? ParseDashboardAiOutput(string rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse))
            return null;

        var trimmed = rawResponse.Trim();
        var json = ExtractJsonObject(trimmed);
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<DashboardAiOutput>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return null;
        }
    }

    private static string? ExtractJsonObject(string text)
    {
        var startIndex = text.IndexOf('{');
        var endIndex = text.LastIndexOf('}');
        if (startIndex < 0 || endIndex <= startIndex)
            return null;

        return text[startIndex..(endIndex + 1)];
    }

    private static List<RevenueForecastServiceItem> BuildFallbackForecast(
        List<MonthlyRevenueServiceItem> historicalRevenue,
        DateTimeOffset endMonth,
        int forecastMonths)
    {
        var last3 = historicalRevenue
            .TakeLast(3)
            .Select(x => x.Revenue)
            .ToList();

        var movingAverage = last3.Count > 0 ? last3.Average() : 0m;
        var result = new List<RevenueForecastServiceItem>(forecastMonths);

        for (int i = 1; i <= forecastMonths; i++)
        {
            var month = endMonth.AddMonths(i);
            result.Add(new RevenueForecastServiceItem(
                $"{month.Year:D4}-{month.Month:D2}",
                decimal.Round(movingAverage, 0),
                "Ước lượng theo trung bình trượt 3 tháng gần nhất."));
        }

        return result;
    }

    private static List<ProductImportSuggestionServiceItem> BuildFallbackSuggestions(
        List<ProductRevenueData> productStats,
        int monthsCount,
        int topProducts)
    {
        if (productStats.Count == 0)
            return [];

        return productStats
            .OrderByDescending(x => x.Revenue)
            .ThenByDescending(x => x.TotalQuantitySold)
            .Take(topProducts)
            .Select(x =>
            {
                var avgMonthlyQty = monthsCount > 0
                    ? (int)Math.Ceiling((decimal)x.TotalQuantitySold / monthsCount)
                    : 1;

                return new ProductImportSuggestionServiceItem(
                    x.ProductId,
                    x.ProductName,
                    Math.Max(1, avgMonthlyQty),
                    "Gợi ý theo doanh thu cao và sản lượng bán ổn định trong lịch sử.");
            })
            .ToList();
    }

    private sealed class OllamaGenerateResponse
    {
        public string? Response { get; set; }
    }

    private sealed class DashboardAiOutput
    {
        public string? Overview { get; set; }
        public List<DashboardAiForecastOutput>? Forecast { get; set; }
        public List<DashboardAiImportSuggestionOutput>? ImportSuggestions { get; set; }
        public List<string>? Actions { get; set; }
    }

    private sealed class DashboardAiForecastOutput
    {
        public string? Month { get; set; }
        public decimal PredictedRevenue { get; set; }
        public string? Reason { get; set; }
    }

    private sealed class DashboardAiImportSuggestionOutput
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int SuggestedQuantity { get; set; }
        public string? Reason { get; set; }
    }
}
