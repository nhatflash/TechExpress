using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Enums;

namespace TechExpress.Service.DTOs.Response
{
    public class UserDetailResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? Birthdate { get; set; }
        public Gender? Gender { get; set; }
        public string? Province { get; set; } 
        public string? Address { get; set; }
        public string? Ward { get; set; }
        public UserStatus Status { get; set; }
        public string? AvatarImage { get; set; }
    }
}
