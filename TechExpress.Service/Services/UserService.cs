using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Contexts;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserContext _userContext;

        public UserService(UnitOfWork unitOfWork, UserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<User> GetMyProfileAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FindUserByIdAsync(userId) ?? throw new NotFoundException("Không tìm thấy người dùng.");

            return user;
        }

        public async Task<User> UpdateMyProfileAsync(UpdateMyProfileDto dto)
        {
            var userId = _userContext.GetCurrentAuthenticatedUserId();
            var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            // Only customer can update
            if (user.Role != UserRole.Customer)
                throw new ForbiddenException("Only customer can update profile.");

            // Update fields that EXIST in domain
            user.Phone = dto.Phone?.Trim();
            user.Gender = dto.Gender;

            user.Province = dto.Province?.Trim();
            user.Ward = dto.Ward?.Trim();

            // StreetAddress maps to Address (domain)
            user.Address = dto.StreetAddress?.Trim();

            // DTO fields requested but domain doesn't have -> commented in DTO, so nothing here.

            await _unitOfWork.SaveChangesAsync();

            return MapToProfileDto(user);
        }



    }
}
