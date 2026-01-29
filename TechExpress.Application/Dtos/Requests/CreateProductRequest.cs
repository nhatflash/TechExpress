using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Name không được để trống")]
    [StringLength(256, ErrorMessage = "Name không được vượt quá 256 ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "SKU không được để trống")]
    [StringLength(100, ErrorMessage = "SKU không được vượt quá 100 ký tự.")]
    public string Sku { get; set; } = string.Empty;

    [Required(ErrorMessage = "CategoryId không được để trống")]
    public Guid CategoryId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price phải > 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "StockQty phải >= 0")]
    public int StockQty { get; set; }

    [Required(ErrorMessage = "Status không được để trống")]
    public ProductStatus Status { get; set; }

    [Required(ErrorMessage = "Description không được để trống")]
    [StringLength(5000, ErrorMessage = "Description không được vượt quá 5000 ký tự.")]
    public string Description { get; set; } = string.Empty;

    public List<string>? Images { get; set; }

    public List<CreateProductSpecValueRequest>? SpecValues { get; set; }
}
