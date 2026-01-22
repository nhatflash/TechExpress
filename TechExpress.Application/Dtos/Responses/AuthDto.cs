using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    string Email,
    UserRole Role
);
