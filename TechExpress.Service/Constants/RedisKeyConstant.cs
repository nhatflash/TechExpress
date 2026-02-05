using System;

namespace TechExpress.Service.Constants;

public static class RedisKeyConstant
{
    public static string ForgotPasswordOtpKey(Guid userId)
    {
        return $"forgot-password-otp:{userId}";
    }
}
