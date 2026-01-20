using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using TechExpress.Application.Common;
using TechExpress.Repository.Contexts;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ApplicationDbContext _db;

        public HealthController(
            IDistributedCache cache,
            ApplicationDbContext db)
        {
            _cache = cache;
            _db = db;
        }

        [HttpGet("/")]
        public IActionResult GetServerHealthStatus()
        {
            return Ok(ApiResponse<string>.OkResponse("Server alive."));
        }

        [HttpGet("redis")]
        public async Task<IActionResult> CheckRedis()
        {
            try
            {
                await _cache.SetStringAsync(
                    "health-check",
                    "ok",
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                    });

                return Ok(ApiResponse<string>.OkResponse("Redis OK"));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    ApiResponse<string>.OkResponse($"Redis DOWN: {ex.Message}")
                );
            }
        }

        [HttpGet("db")]
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                var canConnect = await _db.Database.CanConnectAsync();

                if (!canConnect)
                {
                    return StatusCode(
                        StatusCodes.Status503ServiceUnavailable,
                        ApiResponse<string>.OkResponse("Database unreachable")
                    );
                }

                return Ok(ApiResponse<string>.OkResponse("Database OK"));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    ApiResponse<string>.OkResponse($"Database DOWN: {ex.Message}")
                );
            }
        }
    }
}
