namespace TechExpress.Application.DTOs.Requests
{
    public class UpdateCategoryRequest
    {
        public string? Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool? Status { get; set; }
    }
}
