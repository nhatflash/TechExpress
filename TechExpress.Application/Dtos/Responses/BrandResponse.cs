namespace TechExpress.Application.Dtos.Responses;

public record BrandResponse(
    Guid Id,
    string Name,
    string? ImageUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);

