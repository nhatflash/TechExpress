using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Contexts;
using TechExpress.Service.Enums;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserContext _userContext;
    private readonly IConnectionMultiplexer _redis;

    public UserService(UnitOfWork unitOfWork, UserContext userContext, IConnectionMultiplexer redis)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _redis = redis;
    }

    public async Task<List<User>> HandleGetAllUsers()
    {
        return await _unitOfWork.UserRepository.GetAllUsersAsync();
    }

    public async Task<User> HandleCreateStaff(
        string email,
        string password,
        string? firstName,
        string? lastName,
        string? phone,
        Gender? gender,
        string? address,
        string? ward,
        string? province,
        string? postalCode,
        string? avatarImage,
        string? identity,
        decimal? salary)
    {
        if (await _unitOfWork.UserRepository.UserExistByEmailAsync(email))
        {
            throw new BadRequestException("Email đã tồn tại");
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _unitOfWork.UserRepository.UserExistByPhoneAsync(phone))
            {
                throw new BadRequestException("Số điện thoại đã tồn tại");
            }
        }

        if (!string.IsNullOrWhiteSpace(identity))
        {
            if (await _unitOfWork.UserRepository.UserExistByIdentityAsync(identity!))
            {
                throw new BadRequestException("Số CMND/CCCD đã tồn tại");
            }
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = PasswordEncoder.HashPassword(password),
            Role = UserRole.Staff,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Gender = gender,
            Address = address,
            Ward = ward,
            Province = province,
            PostalCode = postalCode,
            AvatarImage = !string.IsNullOrWhiteSpace(avatarImage) ? avatarImage.Trim() : null,
            Identity = identity,
            Salary = salary,
            Status = UserStatus.Active,
        };

        await _unitOfWork.UserRepository.AddUserAsync(newUser);
        await _unitOfWork.SaveChangesAsync();

        return newUser;
    }

    public async Task<User> HandleGetProfile()
    {
        var userId = _userContext.GetCurrentAuthenticatedUserId();
        var user = await _unitOfWork.UserRepository.FindUserByIdAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

        return user;
    }

    public async Task<User> HandleUpdateProfile(string? firstName, string? lastName, string? phone, Gender? gender, string? address, string? ward, string? province, string? postalCode, string? avatarImage)
    {
        var userId = _userContext.GetCurrentAuthenticatedUserId();
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId) ?? throw new UnauthorizedException("Người dùng không tồn tại.");

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            user.FirstName = firstName;
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            user.LastName = lastName;
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _unitOfWork.UserRepository.UserExistByPhoneAsync(phone) && user.Phone != null && phone != user.Phone)
            {
                throw new BadRequestException("Số điện thoại đã tồn tại.");
            }
            user.Phone = phone;
        }

        if (gender.HasValue)
        {
            user.Gender = gender;
        }

        if (!string.IsNullOrWhiteSpace(province))
        {
            user.Province = province;
        }
        if (!string.IsNullOrWhiteSpace(ward))
        {
            user.Ward = ward;
        }
        if (!string.IsNullOrWhiteSpace(address))
        {
            user.Address = address;
        }

        if (!string.IsNullOrWhiteSpace(postalCode))
        {
            user.PostalCode = postalCode;
        }
        if (!string.IsNullOrWhiteSpace(avatarImage))
        {
            user.AvatarImage = avatarImage;
        }

        await _unitOfWork.SaveChangesAsync();

        return user;
    }


    public async Task<Pagination<User>> HandleGetUsersWithPagination(int pageNumber = 1, int pageSize = 20)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var (users, totalCount) = await _unitOfWork.UserRepository.GetUsersPagedAsync(pageNumber, pageSize);

        return new Pagination<User>
        {
            Items = users,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<User> HandleGetUserDetails(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.FindUserByIdAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

        return user;
    }

    public async Task<User> HandleUpdateUser(Guid userId, string? firstName, string? lastName, string? phone, Gender? gender, string? address, string? ward, string? province, string? postalCode, string? avatarImage)
    {
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            user.FirstName = firstName;
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            user.LastName = lastName;
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _unitOfWork.UserRepository.UserExistByPhoneAsync(phone) && user.Phone != null && phone != user.Phone)
            {
                throw new BadRequestException("Số điện thoại đã tồn tại.");
            }
            user.Phone = phone;
        }

        if (gender.HasValue)
        {
            user.Gender = gender;
        }

        if (!string.IsNullOrWhiteSpace(province))
        {
            user.Province = province;
        }
        if (!string.IsNullOrWhiteSpace(ward))
        {
            user.Ward = ward;
        }
        if (!string.IsNullOrWhiteSpace(address))
        {
            user.Address = address;
        }

        if (!string.IsNullOrWhiteSpace(postalCode))
        {
            user.PostalCode = postalCode;
        }
        if (!string.IsNullOrWhiteSpace(avatarImage))
        {
            user.AvatarImage = avatarImage;
        }

        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task HandleUpdateUserStatus(Guid userId, UserStatus status)
    {
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (user.Status == status)
        {
            throw new BadRequestException("Người dùng đã có trạng thái được chỉ định.");
        }

        user.Status = status;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task HandleDeleteUser(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (user.Status == UserStatus.Deleted)
        {
            throw new BadRequestException("Người dùng đã bị xóa.");
        }

        user.Status = UserStatus.Deleted;
        await _unitOfWork.SaveChangesAsync();
    }


    private async Task UpdateUserWithUpdatedInformation(User user, string? phone, Gender? gender, string? province, string? ward, string? streetAddress)
    {
        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _unitOfWork.UserRepository.UserExistByPhoneAsync(phone) && user.Phone != null && phone != user.Phone)
            {
                throw new BadRequestException("Số điện thoại đã tồn tại.");
            }
            user.Phone = phone.Trim();
        }

        if (gender.HasValue)
        {
            user.Gender = gender;
        }

        if (!string.IsNullOrWhiteSpace(province))
        {
            user.Province = province.Trim();
        }

        if (!string.IsNullOrWhiteSpace(ward))
        {
            user.Ward = ward.Trim();
        }

        if (!string.IsNullOrWhiteSpace(streetAddress))
        {
            user.Address = streetAddress.Trim();
        }

    }

    // Trả về List<User> (Entity gốc)
    public async Task<Pagination<User>> HandleGetStaffListWithPagination(int page, StaffSortBy sortBy)
    {
        const int pageSize = 20;

        // Lấy dữ liệu từ Repo
        List<User> staffs = sortBy switch {
            StaffSortBy.Email => await _unitOfWork.UserRepository.FindStaffsSortByEmailAsync(page, pageSize),
            StaffSortBy.FirstName => await _unitOfWork.UserRepository.FindStaffsSortByFirstNameAsync(page, pageSize),
            _ => await _unitOfWork.UserRepository.FindStaffsSortBySalaryAsync(page, pageSize)
        };
        var totalCount = await _unitOfWork.UserRepository.GetTotalStaffCountAsync();

        return new Pagination<User>
        {
            Items = staffs,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }    
    
    // ======================= Staff_Details=======================//
    public async Task<User> HandleGetStaffDetails(Guid staffId)
    {
        var staff = await _unitOfWork.UserRepository.FindUserByIdAsync(staffId) ?? throw new NotFoundException("Không tìm thấy nhân viên.");
        if (!staff.IsStaffUser())
        {
            throw new BadRequestException("Người dùng không phải là nhân viên.");
        }
        return staff;
    }    

    //================ Update Staff Profile =================//
    public async Task<User> HandleUpdateStaffDetails(Guid staffId, string? firstName, string? lastName, string? phone, string? address, string? ward, string? province, string? identity)
    {
        var user = await _unitOfWork.UserRepository
            .FindUserByIdWithTrackingAsync(staffId)
            ?? throw new UnauthorizedException("Người dùng không tồn tại.");
        if (!user.IsStaffUser())
        {
            throw new BadRequestException("Người dùng không phải là nhân viên.");
        }

        // ========= PHONE =========
        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (user.Phone != null && phone != user.Phone &&await _unitOfWork.UserRepository
                .UserExistByPhoneAsync(phone))
            {
                throw new BadRequestException("Số điện thoại đã tồn tại.");
            }
            user.Phone = phone;
        }

        // ========= IDENTITY =========
        if (!string.IsNullOrWhiteSpace(identity))
        {

            if (user.Identity != null && identity != user.Identity && await _unitOfWork.UserRepository
                .AnyAsync(u => u.Identity == identity && u.Id != staffId))
            {
                throw new BadRequestException("CCCD/CMND đã tồn tại.");
            }
            user.Identity = identity;
        }

        // ========= BASIC FIELDS =========
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            user.FirstName = firstName;
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            user.LastName = lastName;
        }
        if (!string.IsNullOrWhiteSpace(address))
        {
            user.Address = address;
        }
        if (!string.IsNullOrWhiteSpace(ward))
        {
            user.Ward = ward;
        }
        if (!string.IsNullOrWhiteSpace(province))
        {
            user.Province = province;
        }

        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    //================= Remove Staff =================//
    public async Task RemoveStaffAsync(Guid staffId)
    {
        // 1. Tìm và cập nhật Database
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(staffId)
                   ?? throw new NotFoundException("Nhân viên không tồn tại");

        if (!user.IsStaffUser())
            throw new BadRequestException("Chỉ có thể xóa tài khoản staff");

        if (user.Status == UserStatus.Deleted) return;

        user.Status = UserStatus.Deleted;
        await _unitOfWork.SaveChangesAsync();

        // 2. Xóa Cache Redis ngay lập tức để Middleware check lại DB và chặn Staff ngay
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync($"user_status:{staffId}");
    }

}

