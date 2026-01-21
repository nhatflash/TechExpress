using System;
using System.Security.Cryptography;
using System.Text.Json;
using TechExpress.Repository.CustomExceptions;

namespace TechExpress.Service.Utils;

public class OtpUtils
{
    private readonly RedisUtils _redis;
    private static readonly TimeSpan OtpTtl = TimeSpan.FromMinutes(15);

    public OtpUtils(RedisUtils redis)
    {
        _redis = redis;
    }

    
    public async Task<string> CreateAndStoreOtpAsync(string email)
    {
        string emailNorm = NormalizeEmailOrThrow(email);
        string key = OtpRedisKey(emailNorm);

        string otp = GenerateNumericOtp(6);

        var payload = new ForgotPasswordOtpCache
        {
            OtpHash = PasswordEncoder.HashPassword(otp)
        };

        string json = JsonSerializer.Serialize(payload);

        await _redis.StoreStringData(key, json, OtpTtl);

        return otp;
    }

    
    public async Task VerifyOrThrowAsync(string email, string otpInput)
    {
        string emailNorm = NormalizeEmailOrThrow(email);
        string key = OtpRedisKey(emailNorm);

        var json = await _redis.GetStringDataFromKey(key);
        if (string.IsNullOrEmpty(json))
            throw new ForbiddenException("OTP không tồn tại hoặc đã hết hạn.");

        ForgotPasswordOtpCache payload;
        try
        {
            payload = JsonSerializer.Deserialize<ForgotPasswordOtpCache>(json) ?? throw new ForbiddenException("OTP không hợp lệ hoặc đã hết hạn.");
        }
        catch
        {
            throw new ForbiddenException("OTP không hợp lệ hoặc đã hết hạn.");
        }

        if (string.IsNullOrWhiteSpace(otpInput))
            throw new ForbiddenException("OTP không hợp lệ.");

        bool ok = PasswordEncoder.VerifyPassword(otpInput.Trim(), payload.OtpHash);
        if (!ok)
            throw new ForbiddenException("OTP sai hoặc đã hết hạn.");

        await _redis.RemoveAsync(key);
    }

    

    private static string OtpRedisKey(string emailNorm) => $"fp:otp:email:{emailNorm}";

    private static string NormalizeEmailOrThrow(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ForbiddenException("Email không hợp lệ.");

        var e = email.Trim().ToLowerInvariant();

        if (!e.Contains("@"))
            throw new ForbiddenException("Email không hợp lệ.");

        return e;
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

public class ForgotPasswordOtpCache
{
    public string OtpHash { get; set; } = default!;
}
