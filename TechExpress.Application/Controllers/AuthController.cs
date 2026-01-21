using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.Dtos.Requests;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Service;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ServiceProviders _serviceProviders;

        public AuthController(ServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }


        /// <summary>
        /// Chuc nang dang nhap
        /// </summary>

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (user, accessToken, refreshToken) = await _serviceProviders.AuthService.LoginAsyncWithUser(request.Email, request.Password);
            
            var response = ResponseMapper.MapToAuthResponse(accessToken, refreshToken, user);

            return Ok(ApiResponse<AuthResponse>.OkResponse(response));
        }


        /// <summary>
        /// Chuc nang dang ky danh cho customer
        ///</summary>

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
        {
            var (user, accessToken, refreshToken) = await _serviceProviders.AuthService.RegisterAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Phone
            );

            var response = ResponseMapper.MapToAuthResponse(accessToken, refreshToken, user);

            return CreatedAtAction(nameof(Register), ApiResponse<AuthResponse>.CreatedResponse(response));
        }

        /// <summary>
        /// Chuc nang dang ky danh cho staff
        /// </summary>
        [HttpPost("register-staff")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> RegisterStaff([FromBody] RegisterStaffRequest request)
        {
            var (user, accessToken, refreshToken) = await _serviceProviders.AuthService.RegisterStaffAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Phone
            );

            var response = ResponseMapper.MapToAuthResponse(accessToken, refreshToken, user);

            return CreatedAtAction(nameof(RegisterStaff), ApiResponse<AuthResponse>.CreatedResponse(response));
        }
    }
}
