using System;
using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class CreateStaffRequest
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

    [Required(ErrorMessage = "Giới tính không được để trống")]
    public Gender Gender { get; set; }

    [StringLength(256, ErrorMessage = "Địa chỉ không được vượt quá 256 ký tự.")]
    public string? Address { get; set; }

    [StringLength(256, ErrorMessage = "Phuờng/xã không được vượt quá 256 ký tự.")]
    public string? Ward { get; set; }

    [StringLength(256, ErrorMessage = "Tỉnh/thành phố không được vượt quá 256 ký tự.")]
    public string? Province { get; set; }

    [StringLength(20, ErrorMessage = "Mã bưu điện không được vượt quá 20 ký tự.")]
    public string? PostalCode { get; set; }

    public IFormFile? AvatarImage { get; set; }

    [StringLength(20, ErrorMessage = "Số CMND/CCCD không được vượt quá 20 ký tự.")]
    public string? Identity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Tiên lương phải là một số dương.")]
    public decimal? Salary { get; set; }
}
