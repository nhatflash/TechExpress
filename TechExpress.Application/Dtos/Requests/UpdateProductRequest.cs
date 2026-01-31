using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class UpdateProductRequest
{
    [Required(ErrorMessage = "Name không được để trống")]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "SKU không được để trống")]
    [StringLength(100)]
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
    [StringLength(5000)]
    public string Description { get; set; } = string.Empty;


    public List<CreateProductSpecValueRequest>? SpecValues { get; set; }
}
