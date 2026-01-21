using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses
{
    public class StaffDetailResponse
    {
        public string Email { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        public string? Phone { get; set; }


        public string? Address { get; set; }


        public string? Province { get; set; }

        public string? Identity { get; set; }

        public decimal? Salary { get; set; }

        public UserStatus Status { get; set; }
    }
}
