using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses;


public record UserResponse(
    Guid Id, 
    string Email, 
    UserRole Role, 
    string? FirstName, 
    string? LastName, 
    string? Phone, 
    Gender? Gender, 
    string? Address, 
    string? Ward, 
    string? Province, 
    string? PostalCode, 
    string? AvatarImage, 
    string? Identity, 
    decimal? Salary, 
    UserStatus Status,
    DateTimeOffset CreatedAt
);


public record StaffListResponse(
    Guid Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    decimal? Salary,
    UserStatus Status
);


public record StaffDetailResponse(
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Address,
    string? Province,
    string? Identity,
    decimal? Salary,
    UserStatus Status
);


public record UpdateStaffResponse(
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Address,
    string? Ward,
    string? Province,
    string? Identity
);
