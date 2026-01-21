using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _unitOfWork.UserRepository.GetAllUsersAsync();
    }

    public async Task<User> CreateStaffAsync(
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
            throw new BadRequestException("Email already exists");
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _unitOfWork.UserRepository.UserExistByPhoneAsync(phone))
            {
                throw new BadRequestException("Phone number already exists");
            }
        }

        string? avatarImagePath = null;
        if (avatarImage != null && avatarImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(avatarImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new BadRequestException("Invalid file type. Allowed types: jpg, jpeg, png, gif, webp");
            }

            const long maxFileSize = 5 * 1024 * 1024; 
            if (avatarImage.Length > maxFileSize)
            {
                throw new BadRequestException("File size exceeds the maximum limit of 5MB");
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

        await _unitOfWork.UserRepository.CreateUserAsync(newUser);
        await _unitOfWork.SaveChangesAsync();

        return newUser;
    }
}
