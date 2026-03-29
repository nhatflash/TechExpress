using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.IO;
using TechExpress.Application.Common;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Service;

namespace TechExpress.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Staff")]
public class DashboardController : ControllerBase
{
    private readonly ServiceProviders _serviceProvider;

    public DashboardController(ServiceProviders serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Báo cáo doanh thu theo từng tháng.
    /// Có thể filter theo brand/category dựa trên Product của từng OrderItem.
    /// </summary>
    [HttpGet("monthly-revenue")]
    [ProducesResponseType(typeof(ApiResponse<List<MonthlyRevenueItemResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonthlyRevenue(
        [FromQuery] Guid? brandId,
        [FromQuery] Guid? categoryId,
        [FromQuery] DateTimeOffset? fromDate,
        [FromQuery] DateTimeOffset? toDate,
        [FromQuery] bool exportPdf = false,
        CancellationToken ct = default)
    {
        var report = await _serviceProvider.DashboardService.HandleGetMonthlyRevenueAsync(
            brandId,
            categoryId,
            fromDate,
            toDate,
            ct);

        var result = report.Items
            .Select(x => new MonthlyRevenueItemResponse(x.Month, x.Revenue))
            .ToList();

        if (!exportPdf)
            return Ok(ApiResponse<List<MonthlyRevenueItemResponse>>.OkResponse(result));

        var filterText = await _serviceProvider.DashboardService.HandleBuildMonthlyRevenueFilterTextAsync(brandId, categoryId, ct);
        var pdfBytes = GenerateMonthlyRevenuePdf(
            result,
            filterText,
            report.StartMonth,
            report.EndMonth.AddMonths(1).AddTicks(-1));

        return File(pdfBytes, "application/pdf", $"monthly-revenue_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}.pdf");
    }

    private static byte[] GenerateMonthlyRevenuePdf(
        List<MonthlyRevenueItemResponse> data,
        string filterText,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        var maxRevenue = data.Count > 0 ? data.Max(x => x.Revenue) : 0m;
        var barMaxHeight = 160.0f;
        var barColor = Colors.Blue.Lighten2;
        var viCulture = CultureInfo.GetCultureInfo("vi-VN");

        QuestPDF.Settings.License = LicenseType.Community;

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(32);

                page.Content().Column(col =>
                {
                    col.Spacing(6);

                    col.Item().Text("BÁO CÁO DOANH THU THEO THÁNG")
                        .FontSize(18).SemiBold();

                    col.Item().Text(filterText).FontSize(11);
                    col.Item().Text($"Thời gian: {start:yyyy-MM-dd} -> {end:yyyy-MM-dd}").FontSize(11);
                    col.Item().PaddingTop(10);

                    col.Item().Text("Biểu đồ cột doanh thu (theo từng tháng)")
                        .FontSize(12).SemiBold();

                    col.Item().PaddingTop(8)
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .Padding(10)
                        .Column(chartCol =>
                        {
                            chartCol.Item().Row(row =>
                            {
                                row.Spacing(6);

                                foreach (var item in data)
                                {
                                    float barHeight;
                                    if (maxRevenue > 0m)
                                    {
                                        var ratio = (double)(item.Revenue / maxRevenue);
                                        barHeight = (float)(barMaxHeight * ratio);
                                    }
                                    else
                                    {
                                        barHeight = 0f;
                                    }

                                    var visibleBarHeight = Math.Max(2f, barHeight);
                                    var compactRevenue = item.Revenue >= 1_000_000m
                                        ? $"{item.Revenue / 1_000_000m:0.#}M"
                                        : item.Revenue.ToString("N0", viCulture);

                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Height(barMaxHeight + 2)
                                            .AlignBottom()
                                            .AlignCenter()
                                            .Width(20)
                                            .Height(visibleBarHeight)
                                            .Background(barColor)
                                            .Border(0.5f)
                                            .BorderColor(Colors.Blue.Darken1);

                                        c.Item().PaddingTop(4)
                                            .AlignCenter()
                                            .Text(compactRevenue)
                                            .FontSize(7)
                                            .SemiBold();

                                        c.Item().Text(item.Month)
                                            .FontSize(8)
                                            .AlignCenter();
                                    });
                                }
                            });

                            chartCol.Item()
                                .PaddingTop(2)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten2);
                        });

                    col.Item().PaddingTop(14);
                    col.Item().Text("Bảng dữ liệu (Revenue)")
                        .FontSize(12).SemiBold();

                    col.Item().PaddingTop(6).Table(table =>
                    {
                        static IContainer HeaderCellStyle(IContainer container)
                            => container
                                .Background(Colors.Grey.Lighten3)
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten1)
                                .PaddingVertical(6)
                                .PaddingHorizontal(8);

                        static IContainer BodyCellStyle(IContainer container)
                            => container
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(5)
                                .PaddingHorizontal(8);

                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCellStyle).Text("Tháng").SemiBold();
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Doanh thu").SemiBold();
                        });

                        foreach (var item in data)
                        {
                            table.Cell().Element(BodyCellStyle).Text(item.Month);
                            table.Cell().Element(BodyCellStyle).AlignRight().Text(item.Revenue.ToString("N0", viCulture));
                        }
                    });
                });
            });
        });

        using var stream = new MemoryStream();
        doc.GeneratePdf(stream);
        return stream.ToArray();
    }

    /// <summary>
    /// Liệt kê sản phẩm bán chạy nhất và ế nhất kèm tỉ lệ đóng góp vào doanh thu tổng.
    /// Filter theo brand/category dựa trên Product trong OrderItem.
    /// </summary>
    [HttpGet("products/best-worst-selling")]
    [ProducesResponseType(typeof(ApiResponse<ProductsBestWorstSalesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBestWorstSellingProducts(
        [FromQuery] int top = 5,
        [FromQuery] Guid? brandId = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] DateTimeOffset? fromDate = null,
        [FromQuery] DateTimeOffset? toDate = null,
        CancellationToken ct = default)
    {
        if (top < 1 || top > 50)
        {
            return BadRequest(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Value = "top phải trong khoảng 1 đến 50."
            });
        }

        var result = await _serviceProvider.DashboardService.HandleGetBestWorstSellingProductsAsync(
            top,
            brandId,
            categoryId,
            fromDate,
            toDate,
            ct);

        var response = new ProductsBestWorstSalesResponse(
            result.TotalRevenue,
            result.BestSelling.Select(x => new ProductSalesContributionResponse(
                x.ProductId,
                x.ProductName,
                x.TotalQuantitySold,
                x.Revenue,
                x.ContributionRatio)).ToList(),
            result.LeastSelling.Select(x => new ProductSalesContributionResponse(
                x.ProductId,
                x.ProductName,
                x.TotalQuantitySold,
                x.Revenue,
                x.ContributionRatio)).ToList());

        return Ok(ApiResponse<ProductsBestWorstSalesResponse>.OkResponse(response));
    }

    [HttpGet("ai/revenue-insights")]
    [ProducesResponseType(typeof(ApiResponse<DashboardRevenueAiInsightResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAiRevenueInsights(
        [FromQuery] Guid? brandId = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] DateTimeOffset? fromDate = null,
        [FromQuery] DateTimeOffset? toDate = null,
        [FromQuery] int forecastMonths = 3,
        [FromQuery] int topProducts = 5,
        CancellationToken ct = default)
    {
        if (forecastMonths < 1 || forecastMonths > 12)
        {
            return BadRequest(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Value = "forecastMonths phải trong khoảng 1 đến 12."
            });
        }

        if (topProducts < 1 || topProducts > 20)
        {
            return BadRequest(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Value = "topProducts phải trong khoảng 1 đến 20."
            });
        }

        var result = await _serviceProvider.DashboardService.HandleGetAiRevenueInsightsAsync(
            brandId,
            categoryId,
            fromDate,
            toDate,
            forecastMonths,
            topProducts,
            ct);

        var response = new DashboardRevenueAiInsightResponse(
            result.AiGenerated,
            result.Analysis,
            result.Forecast.Select(x => new MonthlyRevenueForecastItemResponse(
                x.Month,
                x.PredictedRevenue,
                x.Reason)).ToList(),
            result.ProductImportSuggestions.Select(x => new ProductImportSuggestionResponse(
                x.ProductId,
                x.ProductName,
                x.SuggestedQuantity,
                x.Reason)).ToList(),
            result.SuggestedActions);

        return Ok(ApiResponse<DashboardRevenueAiInsightResponse>.OkResponse(response));
    }
}

