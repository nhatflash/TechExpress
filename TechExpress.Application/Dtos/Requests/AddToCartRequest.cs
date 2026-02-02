using System;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests
{
    public class AddToCartRequest
    {
        [Required(ErrorMessage = "ProductId là bắt buộc.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
    }
}
