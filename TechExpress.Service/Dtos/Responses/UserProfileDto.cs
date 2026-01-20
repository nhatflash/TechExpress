using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Enums;

namespace TechExpress.Service.Dtos.Responses
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public Gender? Gender { get; set; }

        // public DateTime? Birthdate { get; set; }

        public string? Province { get; set; }

        // public string? City { get; set; }

        // public string? District { get; set; }

        public string? Ward { get; set; }

        public string? StreetAddress { get; set; }
    }
}
