using System;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Application.DTOs.Responses;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Application.Common;

public class ResponseMapper
{
    public static UserResponse MapToUserResponseFromUser(User user)
    {
        return new UserResponse
        (
            user.Id,
            user.Email,
            user.Role,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.Gender,
            user.Address,
            user.Ward,
            user.Province,
            user.PostalCode,
            user.AvatarImage,
            user.Identity,
            user.Salary,
            user.Status,
            user.CreatedAt
        );
    }

    //============= Map to StaffDetailResponse =============//
    public static StaffDetailResponse MapToStaffDetailResponseFromUser(User user)
    {
        return new StaffDetailResponse
        (
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.Address,
            user.Province,
            user.Identity,
            user.Salary,
            user.Status
        );
    }

    public static List<UserResponse> MapToUserResponseListFromUserList(List<User> users)
    {
        return users.Select(MapToUserResponseFromUser).ToList();
    }

    public static AuthResponse MapToAuthResponse(string accessToken, string refreshToken, User user)
    {
        return new AuthResponse
        (
            accessToken,
            refreshToken,
            user.Email,
            user.Role
        );
    }

    public static Pagination<StaffListResponse> MapToStaffListResponsePaginationFromUserPagination(Pagination<User> userPagination)
    {
        var staffResponses = userPagination.Items.Select(MapToStaffListResponseFromUser).ToList();

        return new Pagination<StaffListResponse>
        {
            Items = staffResponses,
            PageNumber = userPagination.PageNumber,
            PageSize = userPagination.PageSize,
            TotalCount = userPagination.TotalCount
        };
    }

    public static StaffListResponse MapToStaffListResponseFromUser(User user)
    {
        return new StaffListResponse
        (
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.Salary,
            user.Status
        );
    }

    public static Pagination<UserResponse> MapToUserResponsePaginationFromUserPagination(Pagination<User> userPagination)
    {
        var userResponses = userPagination.Items.Select(MapToUserResponseFromUser).ToList();

        return new Pagination<UserResponse>
        {
            Items = userResponses,
            PageNumber = userPagination.PageNumber,
            PageSize = userPagination.PageSize,
            TotalCount = userPagination.TotalCount
        };
    }
    //======================= Map Update Staff Response =======================//
    public static UpdateStaffResponse MapToUpdateStaffResponse(User user)
    {
        return new UpdateStaffResponse
        (
            user.FirstName,
            user.LastName,
            user.Phone,
            user.Address,
            user.Ward,
            user.Province,
            user.Identity
        );
    }

    // ======================= Map Category Response từ model => Application.Dtos.response =======================//
    public static CategoryResponse MapToCategoryResponseFromCategory(Category category)
    {
        return new CategoryResponse(
            category.Id,
            category.Name,
            category.ParentCategoryId,
            category.Description,
            category.ImageUrl,
            category.IsDeleted,
            category.CreatedAt,
            category.UpdatedAt
        );
    }
}
