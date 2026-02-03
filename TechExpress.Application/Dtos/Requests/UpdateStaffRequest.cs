namespace TechExpress.Application.Dtos.Requests
{
    public class UpdateStaffRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Ward { get; set; }
        public string? Province { get; set; }
        public string? Identity { get; set; }

        public string? Salary { get; set; }
    }
}
