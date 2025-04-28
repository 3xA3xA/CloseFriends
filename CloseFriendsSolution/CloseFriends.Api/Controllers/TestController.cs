using Microsoft.AspNetCore.Mvc;

namespace CloseFriends.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API работает!");
        }
    }
}
