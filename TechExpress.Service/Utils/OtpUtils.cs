using System;
using System.Security.Cryptography;
using System.Text.Json;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Service.Constants;

namespace TechExpress.Service.Utils;

public class OtpUtils
{
    private readonly RedisUtils _redisUtils;

    private static readonly TimeSpan ResetPasswordOtpExpiryDuration = TimeSpan.FromMinutes(15);

    public OtpUtils(RedisUtils redisUtils)
    {
        _redisUtils = redisUtils;
    }

    
    public async Task<string> CreateAndStoreResetPasswordOtp(Guid userId)
    {
        string otp = GenerateNumericOtp(6);
        var key = RedisKeyConstant.ForgotPasswordOtpKey(userId);

        string hashedOtp = PasswordEncoder.HashPassword(otp);
        await _redisUtils.StoreStringData(key, hashedOtp, ResetPasswordOtpExpiryDuration);

        return otp;
    }
    
    public async Task VerifyResetPasswordOtp(Guid userId, string otpInput)
    {
        var key = RedisKeyConstant.ForgotPasswordOtpKey(userId);

        var storedOtp = await _redisUtils.GetStringDataFromKey(key);
        if (string.IsNullOrEmpty(storedOtp)) 
        {
            throw new ForbiddenException("OTP không tồn tại hoặc đã hết hạn.");
        }

        bool ok = PasswordEncoder.VerifyPassword(otpInput.Trim(), storedOtp);
        if (!ok)
            throw new ForbiddenException("OTP sai hoặc đã hết hạn.");

        await _redisUtils.RemoveAsync(key);
    }

    private static string GenerateNumericOtp(int length)
    {
        var bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        int value = BitConverter.ToInt32(bytes, 0) & int.MaxValue;

        int mod = 1;
        for (int i = 0; i < length; i++) mod *= 10;

        return (value % mod).ToString(new string('0', length));
    }
}
