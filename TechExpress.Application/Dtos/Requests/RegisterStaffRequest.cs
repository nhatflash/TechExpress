using System;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class RegisterStaffRequest
{
    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
    [StringLength(256, ErrorMessage = "Địa chỉ email không được vượt quá 256 ký tự.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string Password { get; set; } = string.Empty;

    [StringLength(256, ErrorMessage = "Tên không được vượt quá 256 ký tự.")]
    public string? FirstName { get; set; }

    [StringLength(256, ErrorMessage = "Họ không được vượt quá 256 ký tự.")]
    public string? LastName { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
    public string? Phone { get; set; }
}
