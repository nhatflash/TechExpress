
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services
{
    public class AuthService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly JwtUtils _jwtUtils;

        public AuthService(UnitOfWork unitOfWork, JwtUtils jwtUtils)
        {
            _unitOfWork = unitOfWork;
            _jwtUtils = jwtUtils;
        }

        public async Task<(User user, string accessToken, string refreshToken)> LoginAsyncWithUser(string email, string password)
        {
            var user = await _unitOfWork.UserRepository.FindUserByEmailAsync(email) ?? throw new UnauthorizedException("Invalid email or password");

            if (!PasswordEncoder.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            if (user.Status != UserStatus.Active)
            {
                throw new ForbiddenException("Your account is not active");
            }

            var accessToken = _jwtUtils.GenerateAccessToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(user);

            return (user, accessToken, refreshToken);
        }

        public async Task<(User user, string accessToken, string refreshToken)> RegisterAsync(
            string email, 
            string password, 
            string? firstName, 
            string? lastName, 
            string? phone)
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

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = PasswordEncoder.HashPassword(password),
                Role = UserRole.Customer,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Status = UserStatus.Active
            };

            await _unitOfWork.UserRepository.CreateUserAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            var accessToken = _jwtUtils.GenerateAccessToken(newUser);
            var refreshToken = _jwtUtils.GenerateRefreshToken(newUser);

            return (newUser, accessToken, refreshToken);
        }

        public async Task<(User user, string accessToken, string refreshToken)> RegisterStaffAsync(
            string email, 
            string password, 
            string? firstName, 
            string? lastName, 
            string? phone)
        {
            var existingUser = await _unitOfWork.UserRepository.FindUserByEmailAsync(email);
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

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = PasswordEncoder.HashPassword(password),
                Role = UserRole.Staff,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Status = UserStatus.Active
            };

            await _unitOfWork.UserRepository.CreateUserAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            var accessToken = _jwtUtils.GenerateAccessToken(newUser);
            var refreshToken = _jwtUtils.GenerateRefreshToken(newUser);

            return (newUser, accessToken, refreshToken);
        }
    
    }
}
