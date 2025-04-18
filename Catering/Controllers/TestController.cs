using Catering.Core.Contracts;
using Catering.Core.Models.Email;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService emailService;

        public TestController(IEmailService _emailService)
        {
            emailService = _emailService;
        }
        [HttpGet("message")]
        //[Authorize]
        public IActionResult Get()
        {
            var message = new Message(new string[] { "kzaraliev@gmail.com" }, "Test email", "This is just a test email");
            emailService.SendEmailAsync(message);
            return Ok(new { message = "API is working!" });
        }
    }
}
