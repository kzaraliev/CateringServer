using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("message")]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(new { message = "API is working!" });
        }
    }
}
