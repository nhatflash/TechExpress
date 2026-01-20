using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechExpress.Service.DTOs.Request;
using TechExpress.Service.DTOs.Response;

namespace TechExpress.Service.Interfaces
{
    public interface IUserService
    {
        Task<Pagination<UserListResponse>> GetUsersAsync(int pageNumber = 1, int pageSize = 20);
        Task<UserDetailResponse> GetUserDetailAsync(Guid userId);
        Task<UserDetailResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task<bool> UpdateUserStatusAsync(Guid userId, UpdateUserStatusRequest request);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
