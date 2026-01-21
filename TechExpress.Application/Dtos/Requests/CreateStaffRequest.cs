using System;
using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class CreateStaffRequest
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

    [Required(ErrorMessage = "Gender is required.")]
    public Gender Gender { get; set; }

    [StringLength(256, ErrorMessage = "Address must not exceed 256 characters.")]
    public string? Address { get; set; }

    [StringLength(256, ErrorMessage = "Ward must not exceed 256 characters.")]
    public string? Ward { get; set; }

    [StringLength(256, ErrorMessage = "Province must not exceed 256 characters.")]
    public string? Province { get; set; }

    [StringLength(20, ErrorMessage = "Postal code must not exceed 20 characters.")]
    public string? PostalCode { get; set; }

    public IFormFile? AvatarImage { get; set; }

    [StringLength(20, ErrorMessage = "Identity must not exceed 20 characters")]
    public string? Identity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
    public decimal? Salary { get; set; }
}
