using Microsoft.AspNetCore.Mvc;

namespace CloseFriends.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTest")]
        public int Get()
        {
            return 0;
        }
    }
}
