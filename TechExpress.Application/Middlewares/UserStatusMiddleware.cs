using StackExchange.Redis;
using TechExpress.Repository;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Middlewares
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;
        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, UnitOfWork unitOfWork, IConnectionMultiplexer redis)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                //  Sửa lại dùng ClaimTypes.NameIdentifier để khớp với JwtRegisteredClaimNames.Sub
                var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;
                    var db = redis.GetDatabase();
                    var cacheKey = $"user_status:{userId}";

                    // 1. Kiểm tra trạng thái trong Redis trước
                    var cachedStatus = await db.StringGetAsync(cacheKey);

                    if (!cachedStatus.IsNull)
                    {
                        if (cachedStatus.ToString() != UserStatus.Active.ToString())
                        {
                            await SendUnauthorizedResponse(context);
                            return;
                        }
                    }
                    else
                    {
                        // 2. Nếu Redis trống, mới truy vấn SQL Server
                        var user = await unitOfWork.UserRepository.FindUserByIdWithNoTrackingAsync(Guid.Parse(userId));

                        if (user == null || user.Status != UserStatus.Active)
                        {
                            // Lưu trạng thái lỗi vào Redis ngắn hạn để chặn nhanh
                            await db.StringSetAsync(cacheKey, user?.Status.ToString() ?? "NotFound", TimeSpan.FromMinutes(10));
                            await SendUnauthorizedResponse(context);
                            return;
                        }

                        // 3. Nếu OK, lưu vào Redis 30 phút để lần sau không cần vào DB
                        await db.StringSetAsync(cacheKey, user.Status.ToString(), TimeSpan.FromMinutes(30));
                    }
                }
            }

            await _next(context);
        }

        private async Task SendUnauthorizedResponse(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Tài khoản đã bị vô hiệu hóa hoặc không tồn tại"
            });
        }
    }
}