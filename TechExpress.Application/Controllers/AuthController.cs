using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Service;
using TechExpress.Service.Dtos;
using TechExpress.Service.Dtos.Requests;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ServiceProviders _sp;

        public AuthController(ServiceProviders sp)
        {
            _sp = sp;
        }

        [HttpPost("forgot-password/request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] ForgotPasswordRequestDto dto, CancellationToken ct)
        {
            bool exists = await _sp.AuthService.EmailExistsAsync(dto.Email);
            if (!exists)
            {
                return Ok(ApiResponse<object>.OkResponse(new
                {
                    message = "If the email exists, an OTP has been sent."
                }));
            }

            var otp = await _sp.ForgotPasswordOtpService.CreateAndStoreOtpAsync(dto.Email);

            var subject = "TechExpress - OTP reset password";
            var html = $@"
                <div style=""font-family: Arial, sans-serif;"">
                    <h3>Reset password</h3>
                    <p>Mã OTP của bạn là:</p>
                    <div style=""font-size: 28px; font-weight: 700; letter-spacing: 4px;"">{otp}</div>
                    <p>Mã có hiệu lực trong <b>15 phút</b>.</p>
                </div>";

            await _sp.EmailSender.SendAsync(dto.Email.Trim().ToLowerInvariant(), subject, html, ct);

            return Ok(ApiResponse<object>.OkResponse(new
            {
                message = "If the email exists, an OTP has been sent."
            }));
        }

        

        [HttpPost("forgot-password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _sp.ForgotPasswordOtpService.VerifyOrThrowAsync(dto.Email, dto.Otp);
            await _sp.AuthService.ResetPasswordByEmailAsync(dto.Email, dto.NewPassword);

            return Ok(ApiResponse<object>.OkResponse(new
            {
                message = "Password reset successful."
            }));
        }
    }
}
