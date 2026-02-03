using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class UpdateProductRequest
{
    [StringLength(256, ErrorMessage = "Tên sản phẩm không được vượt quá 256 ký tự")]
    public string? Name { get; set; }

    [StringLength(100, ErrorMessage = "Mã định danh không được vượt quá 100 ký tự")]
    public string? Sku { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn 0")]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn hoặc bằng 0")]
    public int? Stock { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số tháng bảo hành phải lớn hơn hoặc bằng 0")]
    public int? WarrantyMonth { get; set; }

    public ProductStatus? Status { get; set; }

    [StringLength(5000, ErrorMessage = "Mô tả sản phẩm không được vượt quá 5000 ký tự.")]
    public string? Description { get; set; }

    public List<CreateProductSpecValueRequest> SpecValues { get; set; } = [];
}
