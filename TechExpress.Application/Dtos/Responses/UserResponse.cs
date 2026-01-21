using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public Gender? Gender { get; set; }
    public string? Address { get; set; }
    public string? Ward { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string? AvatarImage { get; set; }
    public string? Identity { get; set; }
    public decimal? Salary { get; set; }
    public UserStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
