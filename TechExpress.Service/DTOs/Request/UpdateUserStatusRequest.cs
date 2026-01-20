using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Service.DTOs.Request
{
    public class UpdateUserStatusRequest
    {
        [Required]
        public UserStatus Status { get; set; }
    }
}