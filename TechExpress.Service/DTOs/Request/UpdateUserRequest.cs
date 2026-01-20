using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Service.DTOs.Request
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public Gender? Gender { get; set; }

        public string? Address { get; set; }

        public string? Ward { get; set; }

        public string? Province { get; set; }

        public string? PostalCode { get; set; }

        public string? AvatarImage { get; set; }
    }
}