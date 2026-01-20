using Microsoft.AspNetCore.Mvc;
using TechExpress.Service;

namespace TechExpress.Application.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ServiceProviders _sp;

        public TestController(ServiceProviders sp)
        {
            _sp = sp;
        }

        // remove when sign up complete
        [HttpPost("insert-2-user")]
        public async Task<IActionResult> Insert2User()
        {
            var result = await _sp.UserService.InsertTwoTestUsersAsync();
            return Ok(result);
        }
    }
}
