using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(256, ErrorMessage = "Tên sản phẩm không được vượt quá 256 ký tự.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Mã định danh không được để trống")]
    [StringLength(100, ErrorMessage = "Mã định danh không được vượt quá 100 ký tự.")]
    public required string Sku { get; set; }

    [Required(ErrorMessage = "Danh mục không được để trống")]
    public required Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Tên hãng không được để trống")]
    public required Guid BrandId { get; set; }

    [Required(ErrorMessage = "Giá tiền không được để trống")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn 0")]
    public required decimal Price { get; set; }

    [Required(ErrorMessage = "Số lượng sản phẩm không được để trống")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn hoặc bằng 0")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Số tháng bảo hành không được để trống")]
    [Range(0, int.MaxValue, ErrorMessage = "Số tháng bảo hành phải lớn hơn hoặc bằng 0")]
    public required int WarrantyMonth { get; set; }

    [Required(ErrorMessage = "Mô tả không được để trống")]
    [StringLength(5000, ErrorMessage = "Mô tả không được vượt quá 5000 ký tự.")]
    public required string Description { get; set; }

    public List<string> Images { get; set; } = [];

    public List<CreateProductSpecValueRequest> SpecValues { get; set; } = [];
}
