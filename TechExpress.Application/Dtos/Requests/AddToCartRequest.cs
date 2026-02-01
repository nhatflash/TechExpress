using System;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests
{
    public class AddToCartRequest
    {
        [Required(ErrorMessage = "ProductId là b?t bu?c.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity là b?t bu?c.")]
        [Range(1, int.MaxValue, ErrorMessage = "S? l??ng ph?i l?n h?n 0.")]
        public int Quantity { get; set; }
    }
}
