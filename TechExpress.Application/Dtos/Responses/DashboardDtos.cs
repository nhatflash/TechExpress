using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses;

public record MonthlyRevenueItemResponse(
    string Month,
    decimal Revenue
);

public record ProductSalesContributionResponse(
    Guid ProductId,
    string ProductName,
    int TotalQuantitySold,
    decimal Revenue,
    decimal ContributionRatio
);

public record ProductsBestWorstSalesResponse(
    decimal TotalRevenue,
    List<ProductSalesContributionResponse> BestSelling,
    List<ProductSalesContributionResponse> LeastSelling
);

public record MonthlyRevenueForecastItemResponse(
    string Month,
    decimal PredictedRevenue,
    string Reason
);

public record ProductImportSuggestionResponse(
    Guid ProductId,
    string ProductName,
    int SuggestedQuantity,
    string Reason
);

public record DashboardRevenueAiInsightResponse(
    bool AiGenerated,
    string Analysis,
    List<MonthlyRevenueForecastItemResponse> Forecast,
    List<ProductImportSuggestionResponse> ProductImportSuggestions,
    List<string> SuggestedActions
);

