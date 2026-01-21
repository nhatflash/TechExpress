using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechExpress.Application.Common;
using TechExpress.Service;
using TechExpress.Service.Dtos.Requests;
using TechExpress.Service.Dtos.Responses;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ServiceProviders _serviceProvider;

        public UserController(ServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        [HttpGet("me/profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();

            var profile = await _serviceProvider.UserService.GetMyProfileAsync(userId);

            return Ok(ApiResponse<UserProfileDto>.OkResponse(profile));
        }


        [HttpPut("me/profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileRequest request)
        {
            var userId = GetCurrentUserId();

            var updated = await _sp.UserService.UpdateMyProfileAsync(userId, dto);

            return Ok(ApiResponse<UserProfileDto>.OkResponse(updated));
        }

        private Guid GetCurrentUserId()
        {
            throw new NotImplementedException();
        }
    }
}
