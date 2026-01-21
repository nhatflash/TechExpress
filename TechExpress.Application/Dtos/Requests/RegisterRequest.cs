using System;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256, ErrorMessage = "Email must not exceed 256 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [StringLength(256, ErrorMessage = "Password must not exceed 256 characters.")]
    public string Password { get; set; } = string.Empty;

    [StringLength(256, ErrorMessage = "First name must not exceed 256 characters.")]
    public string? FirstName { get; set; }

    [StringLength(256, ErrorMessage = "Last name must not exceed 256 characters.")]
    public string? LastName { get; set; }

    [Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters.")]
    public string? Phone { get; set; }
}
