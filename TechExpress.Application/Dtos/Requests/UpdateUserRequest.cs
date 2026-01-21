using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests
{
    public class UpdateUserRequest
    {
        [StringLength(256, ErrorMessage = "Độ dài tên không được vượt quá 256 ký tự.")]
        public string? FirstName { get; set; }

        [StringLength(256, ErrorMessage = "Độ dài họ không được vượt quá 256 ký tự.")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(20, ErrorMessage = "Độ dài số điện thoại không được vượt quá 20 ký tự.")]
        public string? Phone { get; set; }

        public Gender? Gender { get; set; }

        [StringLength(256, ErrorMessage = "Độ dài địa chỉ không được vượt quá 256 ký tự.")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "Độ dài phường/xã không được vượt quá 100 ký tự.")]
        public string? Ward { get; set; }

        [StringLength(100, ErrorMessage = "Độ dài tỉnh/thành phố không được vượt quá 100 ký tự.")]
        public string? Province { get; set; }

        [StringLength(20, ErrorMessage = "Độ dài mã bưu chính không được vượt quá 20 ký tự.")]
        public string? PostalCode { get; set; }

        public string? AvatarImage { get; set; }
    }
}