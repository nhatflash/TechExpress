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
        private readonly ApplicationDbContext _db;

        public HealthController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("/")]
        public IActionResult GetServerHealthStatus()
        {
            return Ok(ApiResponse<string>.OkResponse("Máy chủ đang hoạt động"));
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
                        ApiResponse<string>.OkResponse("Không thể kết nối đến cơ sở dữ liệu")
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
