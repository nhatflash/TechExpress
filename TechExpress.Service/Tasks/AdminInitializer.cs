using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechExpress.Repository;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Tasks;

public class AdminInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminInitializer> _logger;

    public AdminInitializer(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<AdminInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var adminEmail = _configuration["AdminUser:Email"];
            var adminPassword = _configuration["AdminUser:Password"];
            var adminFirstName = _configuration["AdminUser:FirstName"];
            var adminLastName = _configuration["AdminUser:LastName"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                _logger.LogWarning("Cấu hình người dùng admin không đầy đủ. Bỏ qua khởi tạo người dùng admin.");
                return;
            }

            if (await unitOfWork.UserRepository.UserExistByEmailAsync(adminEmail))
            {
                _logger.LogInformation("Hệ thống đã có người dùng quản trị với email {}. Bỏ qua khởi tạo.", adminEmail);
                return;
            }

            if (await unitOfWork.UserRepository.UserExistByRoleAsync(UserRole.Admin))
            {
                _logger.LogInformation("Hệ thống đã có người dùng quản trị. Bỏ qua khởi tạo.");
                return;
            }

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                PasswordHash = PasswordEncoder.HashPassword(adminPassword),
                Role = UserRole.Admin,
                FirstName = adminFirstName,
                LastName = adminLastName,
                Status = UserStatus.Active
            };

            await unitOfWork.UserRepository.AddUserAsync(adminUser);
            await unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Người dùng admin đã được tạo thành công: {}", adminEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi khởi tạo người dùng admin.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
