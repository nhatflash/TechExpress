using System;
using System.Threading.Tasks;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services
{
    public class AuthService
    {
        private readonly UnitOfWork _unitOfWork;

        public AuthService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ResetPasswordByEmailAsync(string email, string newPassword)
        {
            var emailNorm = NormalizeEmailOrThrow(email);

            var user = await _unitOfWork.UserRepository.FindUserByEmailWithTrackingAsync(emailNorm);
            if (user == null)
                throw new ForbiddenException("Tài khoản không tồn tại.");

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
                throw new ForbiddenException("Mật khẩu không hợp lệ (tối thiểu 8 ký tự).");

            user.PasswordHash = PasswordEncoder.HashPassword(newPassword);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var emailNorm = NormalizeEmailOrThrow(email);
            var user = await _unitOfWork.UserRepository.FindUserByEmailAsync(emailNorm);
            return user != null;
        }

        private static string NormalizeEmailOrThrow(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ForbiddenException("Email không hợp lệ.");

            var e = email.Trim().ToLowerInvariant();
            if (!e.Contains("@"))
                throw new ForbiddenException("Email không hợp lệ.");

            return e;
        }
    }
}
