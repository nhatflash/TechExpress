using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class RefreshNewTokenRequest
{
    [Required(ErrorMessage = "Refresh token là bắt buộc")]
    public required string RefreshToken { get; set; }
}