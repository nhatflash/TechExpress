using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests
{
    public class UpdateCartItemRequest
    {
        [Required(ErrorMessage = "Quantity là b?t bu?c.")]
        [Range(0, int.MaxValue, ErrorMessage = "S? l??ng không ???c âm.")]
        public int Quantity { get; set; }
    }
}
