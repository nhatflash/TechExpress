using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Dtos.Requests;
using TechExpress.Service.Dtos.Responses;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProfileDto> GetMyProfileAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FindUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            return MapToProfileDto(user);
        }

        public async Task<UserProfileDto> UpdateMyProfileAsync(Guid userId, UpdateMyProfileDto dto)
        {
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

        private static UserProfileDto MapToProfileDto(User user)
        {
            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,

                Phone = user.Phone,
                Gender = user.Gender,

                Province = user.Province,
                Ward = user.Ward,

                StreetAddress = user.Address
            };
        }



    }
}
