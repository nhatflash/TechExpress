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
        public async Task InvokeAsync(HttpContext context, UnitOfWork unitOfWork)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // Sử dụng ClaimTypes.NameIdentifier để khớp với JwtRegisteredClaimNames.Sub
                var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userId = Guid.Parse(userIdClaim.Value);

                    var user = await unitOfWork.UserRepository.FindUserByIdWithNoTrackingAsync(userId);

                    if (user == null || user.Status != UserStatus.Active)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { error = "Tài khoản đã bị vô hiệu hóa" });
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
