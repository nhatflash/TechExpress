using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly ServiceProviders _serviceProviders;

        public UserController(ServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Lấy danh sách tất cả user (chỉ Admin)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _serviceProviders.UserService.GetAllUsersAsync();

            var response = ResponseMapper.MapToUserResponseListFromUserList(users);

            return Ok(ApiResponse<List<UserResponse>>.OkResponse(response));
        }

        /// <summary>
        /// Tạo staff mới (chỉ Admin)
        /// </summary>
        [HttpPost("create-staff")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateStaff([FromForm] CreateStaffRequest request)
        {
            var user = await _serviceProviders.UserService.CreateStaffAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Gender,
                request.Address,
                request.Ward,
                request.Province,
                request.PostalCode,
                request.AvatarImage,
                request.Identity,
                request.Salary
            );

            var response = ResponseMapper.MapToUserResponseFromUser(user);

            return CreatedAtAction(nameof(CreateStaff), ApiResponse<UserResponse>.CreatedResponse(response));
        }
    }

}
