using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TechExpress.Repository.Enums;

namespace TechExpress.Repository.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public required UserRole Role { get; set; }

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

        public decimal? Salary { get; set;  }

        public required UserStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;


        public bool IsStaffUser()
        {
            return Role == UserRole.Staff;
        }

        public bool IsAdminUser()
        {
            return Role == UserRole.Admin;
        }

        public bool IsCustomerUser()
        {
            return Role == UserRole.Customer;
        }

    }
}
