using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests
{
    public class UpdateUserStatusRequest
    {
        [Required(ErrorMessage = "Trạng thái người dùng là bắt buộc.")]
        public UserStatus Status { get; set; }
    }
}