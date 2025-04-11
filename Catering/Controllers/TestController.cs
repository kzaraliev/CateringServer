using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("message")]
        public IActionResult Get()
        {
            return Ok(new { message = "API is working!" });
        }
    }
}
