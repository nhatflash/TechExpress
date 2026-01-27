using System.ComponentModel.DataAnnotations;

namespace TechExpress.Service.DTOs.Requests
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        public  string Name { get; set; } = string.Empty;

        public Guid? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "Mô tả danh mục không được để trống")]
        public  string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}
