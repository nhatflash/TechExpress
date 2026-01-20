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
    public class UsersController : ControllerBase
    {
        private readonly ServiceProviders _sp;

        public UsersController(ServiceProviders sp)
        {
            _sp = sp;
        }


        [HttpGet("me/profile")]
        public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        {
            var userId = GetCurrentUserId();

            var profile = await _sp.UserService.GetMyProfileAsync(userId);

            return Ok(ApiResponse<UserProfileDto>.OkResponse(profile));
        }


        [HttpPut("me/profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileDto dto, CancellationToken ct)
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
