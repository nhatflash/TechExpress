using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Contexts;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserContext _userContext;

    public UserService(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, UserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _userContext = userContext;
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
        IFormFile? avatarImage,
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

        string? avatarImagePath = null;
        if (avatarImage != null && avatarImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(avatarImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new BadRequestException("Loại tệp tin không hợp lệ. Các loại cho phép: jpg, jpeg, png, gif, webp");
            }

            const long maxFileSize = 5 * 1024 * 1024; 
            if (avatarImage.Length > maxFileSize)
            {
                throw new BadRequestException("Tệp tin quá lớn. Kích thước tối đa cho phép là 5MB.");
            }

            // Ensure wwwroot exists
            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
                if (!Directory.Exists(webRootPath))
                {
                    Directory.CreateDirectory(webRootPath);
                }
            }

            var uploadsFolder = Path.Combine(webRootPath, "uploads", "avatars");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarImage.CopyToAsync(stream);
            }

            // Get base URL from HttpContext
            var httpContext = _httpContextAccessor.HttpContext;
            var baseUrl = httpContext != null 
                ? $"{httpContext.Request.Scheme}://{httpContext.Request.Host}"
                : "https://localhost:7194"; // Fallback if HttpContext is not available

            avatarImagePath = $"{baseUrl}/uploads/avatars/{fileName}";
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
            AvatarImage = avatarImagePath,
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

    public async Task<User> HandleUpdateProfile(string? phone, Gender? gender, string? province, string? ward, string? streetAddress)
    {
        var userId = _userContext.GetCurrentAuthenticatedUserId();
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId) ?? throw new UnauthorizedException("Người dùng không tồn tại.");

        await UpdateUserWithUpdatedInformation(user, phone, gender, province, ward, streetAddress);

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

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (user.Status == UserStatus.Deleted)
        {
            throw new BadRequestException("Người dùng đã bị xóa.");
        }

        user.Status = UserStatus.Deleted;
        await _unitOfWork.SaveChangesAsync();
    }


    private async Task UpdateUserWithUpdatedInformation(User user,string? phone, Gender? gender, string? province, string? ward, string? streetAddress)
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



}


