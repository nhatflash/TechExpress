using TechExpress.Repository;
using TechExpress.Service.Services;
using TechExpress.Service.Services.Otp;
using TechExpress.Service.Utils;
using Viren.Services.Impl;

namespace TechExpress.Service
{
    public class ServiceProviders
    {
        public AuthService AuthService { get; }
        public UserService UserService { get; }

        public ForgotPasswordOtpService ForgotPasswordOtpService { get; }
        public SmtpEmailSender EmailSender { get; }

        public ServiceProviders(UnitOfWork unitOfWork, RedisUtils redis, SmtpEmailSender emailSender)
        {
            AuthService = new AuthService(unitOfWork);
            UserService = new UserService(unitOfWork);

            ForgotPasswordOtpService = new ForgotPasswordOtpService(redis);
            EmailSender = emailSender;
        }
    }
}
