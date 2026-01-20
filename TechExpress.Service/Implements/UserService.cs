using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechExpress.Repository;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.DTOs.Request;
using TechExpress.Service.DTOs.Response;
using TechExpress.Service.Exceptions;
using TechExpress.Service.Interfaces;

namespace TechExpress.Service.Implements
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<UserListResponse>> GetUsersAsync(int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var (users, totalCount) = await _unitOfWork.UserRepository.GetUsersPagedAsync(pageNumber, pageSize);

            var userListDtos = users.Select(u => new UserListResponse
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.Phone,
                TotalSpend = 0m, // TODO: Calculate from Orders table
                Status = u.Status
            }).ToList();

            return new Pagination<UserListResponse>
            {
                Items = userListDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<UserDetailResponse> GetUserDetailAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserDetailByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return new UserDetailResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.Phone,
                Gender = user.Gender,
                Province = user.Province,
                Address = user.Address,
                Ward = user.Ward,
                Status = user.Status,
                AvatarImage = user.AvatarImage
            };
        }

        public async Task<UserDetailResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (request.FirstName != null)
                user.FirstName = request.FirstName;

            if (request.LastName != null)
                user.LastName = request.LastName;

            if (request.Phone != null)
                user.Phone = request.Phone;

            if (request.Gender.HasValue)
                user.Gender = request.Gender;

            if (request.Province != null)
                user.Province = request.Province;

            if (request.Address != null)
                user.Address = request.Address;

            if (request.Ward != null)
                user.Ward = request.Ward;

            if (request.PostalCode != null)
                user.PostalCode = request.PostalCode;

            if (request.AvatarImage != null)
                user.AvatarImage = request.AvatarImage;

            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserDetailResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.Phone,
                Gender = user.Gender,
                Province = user.Province,
                Address = user.Address,
                Ward = user.Ward,
                Status = user.Status,
                AvatarImage = user.AvatarImage
            };
        }

        public async Task<bool> UpdateUserStatusAsync(Guid userId, UpdateUserStatusRequest request)
        {
            var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.Status = request.Status;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FindUserByIdWithTrackingAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.Status = UserStatus.Deleted;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
