using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Service;
using TechExpress.Service.DTOs.Request;
using TechExpress.Service.DTOs.Response;
using TechExpress.Service.Interfaces;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get paginated list of users (20 records per page by default)
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 20, max: 100)</param>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<Pagination<UserListResponse>>>> GetUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _userService.GetUsersAsync(pageNumber, pageSize);
            return Ok(ApiResponse<Pagination<UserListResponse>>.OkResponse(result));
        }

        /// <summary>
        /// Get detailed information of a specific user
        /// </summary>
        /// <param name="id">User ID</param>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDetailResponse>>> GetUserDetail(Guid id)
        {
            var result = await _userService.GetUserDetailAsync(id);
            return Ok(ApiResponse<UserDetailResponse>.OkResponse(result));
        }

        /// <summary>
        /// Update user profile information
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">User update data</param>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDetailResponse>>> UpdateUser(
            Guid id,
            [FromBody] UpdateUserRequest request)
        {
            var result = await _userService.UpdateUserAsync(id, request);
            return Ok(ApiResponse<UserDetailResponse>.OkResponse(result));
        }

        /// <summary>
        /// Update user status
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">Status update data</param>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUserStatus(
            Guid id,
            [FromBody] UpdateUserStatusRequest request)
        {
            var result = await _userService.UpdateUserStatusAsync(id, request);
            return Ok(ApiResponse<bool>.OkResponse(result));
        }

        /// <summary>
        /// Delete a user (soft delete - sets status to Deleted)
        /// </summary>
        /// <param name="id">User ID</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return Ok(ApiResponse<bool>.OkResponse(result));
        }
    }
}
