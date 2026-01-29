using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class CreateBrandRequest
{
    [Required(ErrorMessage = "Tên không được để trống")]
    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2048, ErrorMessage = "URL ảnh không được vượt quá 2048 ký tự")]
    public string? ImageUrl { get; set; }
}

