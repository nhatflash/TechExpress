using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses
{
    public record ProductListResponse(
    Guid Id,
    string Name,
    string Sku,
    Guid CategoryId,
    string CategoryName,
    decimal Price,
    int StockQty,
    ProductStatus Status,
    string? FirstImageUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);

    public record ProductSpecValueResponse(
    Guid SpecDefinitionId,
    string SpecName,
    string Unit,
    SpecAcceptValueType DataType,
    string Value
);

    public record ProductDetailResponse(
        Guid Id,
        string Name,
        string Sku,
        Guid CategoryId,
        string CategoryName,
        decimal Price,
        int StockQty,
        ProductStatus Status,
        string Description,
        string? ThumbnailUrl,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt,
        List<ProductSpecValueResponse> SpecValues
    );
}
