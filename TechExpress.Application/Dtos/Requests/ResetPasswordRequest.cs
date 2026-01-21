using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechExpress.Application.Dtos.Requests
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "OTP là bắt buộc.")]
        public string Otp { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu mới là bắt buộc.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
