using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        [HttpGet("/")]
        public IActionResult GetServerHealthStatus()
        {
            var response = ApiResponse<string>.OkResponse("Server alive.");
            return Ok(response);
        }
    }
}
