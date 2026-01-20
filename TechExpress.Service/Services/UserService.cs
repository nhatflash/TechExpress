using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Models;
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

        public async Task<object> InsertTwoTestUsersAsync()
        {
            var emails = new[]
            {
                "thongduy06@gmail.com",
                "duythongtrannguyen98@gmail.com"
            };

            int inserted = 0;
            int skipped = 0;

            foreach (var rawEmail in emails)
            {
                var email = NormalizeEmailOrThrow(rawEmail);

                var exists = await _unitOfWork.UserRepository.FindUserByEmailAsync(email);
                if (exists != null)
                {
                    skipped++;
                    continue;
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    PasswordHash = PasswordEncoder.HashPassword("Test@1234"),

                    // Bạn đổi theo enum thực tế của bạn
                    // Role = UserRole.Customer,
                    // Status = UserStatus.Active
                };

                await _unitOfWork.UserRepository.AddAsync(user);
                inserted++;
            }

            await _unitOfWork.SaveChangesAsync();

            return new
            {
                message = "Seed completed",
                inserted,
                skipped
            };
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
