using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses
{
    public class UpdateStaffResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Ward { get; set; }
        public string? Province { get; set; }

        public string? Identity { get; set; }
    }
}
