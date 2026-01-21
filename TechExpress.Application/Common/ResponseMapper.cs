using System;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Application.Common;

public class ResponseMapper
{
public static UserResponse MapToUserResponseFromUser(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Gender = user.Gender,
            Address = user.Address,
            Ward = user.Ward,
            Province = user.Province,
            PostalCode = user.PostalCode,
            AvatarImage = user.AvatarImage,
            Identity = user.Identity,
            Salary = user.Salary,
            Status = user.Status,
            CreatedAt = user.CreatedAt
        };
    }

    public static List<UserResponse> MapToUserResponseListFromUserList(List<User> users)
    {
        return users.Select(MapToUserResponseFromUser).ToList();
    }

    public static AuthResponse MapToAuthResponse(string accessToken, string refreshToken, User user)
    {
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Email = user.Email,
            Role = user.Role.ToString(),
        };
    }

    public static Pagination<StaffResponse> MapToStaffResponsePaginationFromUserPagination(Pagination<User> userPagination)
    {
        var staffResponses = userPagination.Items.Select(MapToStaffResponseFromUser).ToList();

        return new Pagination<StaffResponse>
        {
            Items = staffResponses,
            PageNumber = userPagination.PageNumber,
            PageSize = userPagination.PageSize,
            TotalCount = userPagination.TotalCount
        };
    }

    public static StaffResponse MapToStaffResponseFromUser(User user)
    {
        return new StaffResponse
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Salary = user.Salary,
            Status = user.Status
        };
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
}
