using System;

namespace TechExpress.Service.Constants;

public class RedisKeyConstant
{
    public static string ForgotPasswordOtpKey(Guid userId)
    {
        return $"forgot-password-otp:{userId}";
    }
}
