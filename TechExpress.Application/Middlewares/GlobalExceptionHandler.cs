using Microsoft.AspNetCore.Diagnostics;
using TechExpress.Application.Common;
using TechExpress.Repository.CustomExceptions;

namespace TechExpress.Application.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger; 
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                BaseException customEx => (customEx.StatusCode, customEx.Message),
                _ => (StatusCodes.Status500InternalServerError, $"Lỗi không xác định")
            };
            if (exception is not BaseException)
            {
                _logger.LogError(exception, "Unhandled exception at {Path}", httpContext.Request.Path);
            }

            httpContext.Response.StatusCode = statusCode;

            var errResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message
            };

            await httpContext.Response.WriteAsJsonAsync(errResponse, cancellationToken);

            return true;
        }
    }
}
