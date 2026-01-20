using TechExpress.Repository.Enums;

namespace TechExpress.Service.DTOs.Response
{
    public class UserListResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal TotalSpend { get; set; }
        public UserStatus Status { get; set; }
    }
}