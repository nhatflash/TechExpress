using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.Dtos.Requests;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Repository.Enums;
using TechExpress.Service;
using TechExpress.Service.Utils;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ServiceProviders _serviceProvider;

        public UserController(ServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Lấy danh sách tất cả user (chỉ Admin)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _serviceProvider.UserService.HandleGetAllUsers();

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
            var user = await _serviceProvider.UserService.HandleCreateStaff(
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

        /// <summary>
        /// Get paginated list of users (20 records per page by default)
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 20, max: 100)</param>
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetUsersWithPagination(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var userPagination = await _serviceProvider.UserService.HandleGetUsersWithPagination(pageNumber, pageSize);
            var response = ResponseMapper.MapToUserResponsePaginationFromUserPagination(userPagination);
            return Ok(ApiResponse<Pagination<UserResponse>>.OkResponse(response));
        }

        /// <summary>
        /// Get detailed information of a specific user
        /// </summary>
        /// <param name="id">User ID</param>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetUserDetail(Guid id)
        {
            var user = await _serviceProvider.UserService.HandleGetUserDetails(id);
            var response = ResponseMapper.MapToUserResponseFromUser(user);
            return Ok(ApiResponse<UserResponse>.OkResponse(response));
        }

        /// <summary>
        /// Update user profile information
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">User update data</param>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUser(
            Guid id,
            [FromBody] UpdateUserRequest request)
        {
            var user = await _serviceProvider.UserService.HandleUpdateUser(id, request.FirstName, request.LastName, request.Phone, request.Gender, request.Address, request.Ward, request.Province, request.PostalCode, request.AvatarImage);

            var response = ResponseMapper.MapToUserResponseFromUser(user);
            return Ok(ApiResponse<UserResponse>.OkResponse(response));
        }

        /// <summary>
        /// Update user status
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">Status update data</param>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatus(
            Guid id,
            [FromBody] UpdateUserStatusRequest request)
        {
            await _serviceProvider.UserService.HandleUpdateUserStatus(id, request.Status);

            return Ok(ApiResponse<string>.OkResponse($"Trạng thái người dùng đã được cập nhật thành {request.Status}."));
        }

        /// <summary>
        /// Delete a user (soft delete - sets status to Deleted)
        /// </summary>
        /// <param name="id">User ID</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _serviceProvider.UserService.DeleteUserAsync(id);
            return Ok(ApiResponse<string>.OkResponse("Người dùng đã được xóa thành công."));
        }
    
    //========================Staff List =========================//
        [Authorize(Roles = "Admin")]
        [HttpGet("staffs")]
        public async Task<IActionResult> GetStaffList(
            [FromQuery] int page = 1,
            [FromQuery] StaffSortBy sortBy = StaffSortBy.Email)
        {
            if (page < 1)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Page must be greater than 0"
                });
            }

            // 1. Lấy dữ liệu gốc (List<User>)
            var staffEntities = await _serviceProvider.UserService
                .GetStaffListAsync(page, sortBy);

            // 2. MAP DỮ LIỆU (Tối ưu nhất)
            // Cú pháp này gọi là "Method Group".
            // Nó tự động lấy từng User trong list ném vào hàm MapToStaffListResponse
            var staffDtos = staffEntities
                            .Select(ResponseMapper.MapToStaffListResponse)
                            .ToList();

            // 3. Trả về
            var response = ApiResponse<object>.OkResponse(new
            {
                Page = page,
                PageSize = 20,
                SortBy = sortBy.ToString(),
                Data = staffDtos
            });

            return Ok(response);
        }
    }
}
