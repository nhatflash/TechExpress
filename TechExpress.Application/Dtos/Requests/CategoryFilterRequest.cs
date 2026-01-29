namespace TechExpress.Application.DTOs.Requests
{
    public class CategoryFilterRequest
    {
        public string? SearchName { get; set; }        // Tìm theo Name
        public Guid? ParentId { get; set; }        // Lọc theo ParentId
        public bool? Status { get; set; }          // Lọc theo Status
        public int Page { get; set; } = 1;         // Trang hiện tại
    }
}
