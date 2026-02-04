using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests
{
    public class UpdateCartItemRequest
    {
        [Required(ErrorMessage = "Quantity là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm.")]
        public int Quantity { get; set; }
    }
}
