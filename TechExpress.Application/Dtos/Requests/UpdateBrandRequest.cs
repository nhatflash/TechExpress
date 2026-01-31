using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class UpdateBrandRequest
{
    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string? Name { get; set; }

    [StringLength(2048, ErrorMessage = "URL ảnh không được vượt quá 2048 ký tự")]
    public string? ImageUrl { get; set; }
}

