using TechExpress.Repository.Models;

namespace TechExpress.Application.DTOs.Responses
{
    public record CategoryResponse(
        Guid Id,
        string Name,
        Guid? ParentCategoryId,
        string Description,
        string? ImageUrl,
        bool IsDeleted,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );
}
