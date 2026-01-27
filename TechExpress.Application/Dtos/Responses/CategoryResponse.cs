using TechExpress.Repository.Models;

namespace TechExpress.Application.DTOs.Responses
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }

        public  string Name { get; set; }

        public Guid? ParentCategoryId { get; set; }

        public  string Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
    }
}
