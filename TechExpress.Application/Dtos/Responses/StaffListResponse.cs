using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses
{
    public class StaffListResponse
    {
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public decimal? Salary { get; set; }
        public UserStatus Status { get; set; }
    }
}
