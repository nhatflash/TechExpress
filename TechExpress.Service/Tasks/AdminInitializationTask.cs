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

public class AdminInitializationTask : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminInitializationTask> _logger;

    public AdminInitializationTask(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<AdminInitializationTask> logger)
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
                _logger.LogWarning("Admin user configuration is missing. Skipping admin initialization.");
                return;
            }

            if (await unitOfWork.UserRepository.UserExistByEmailAsync(adminEmail))
            {
                _logger.LogInformation("Admin user already exists. Skipping initialization.");
                return;
            }

            if (await unitOfWork.UserRepository.UserExistByRoleAsync(UserRole.Admin))
            {
                _logger.LogInformation("Admin user already exists in database. Skipping initialization.");
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

            await unitOfWork.UserRepository.CreateUserAsync(adminUser);
            await unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Admin user created successfully: {Email}", adminEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while initializing admin user");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
