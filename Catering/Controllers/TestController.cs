using Catering.Core.Contracts;
using Catering.Core.Models.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

        [HttpGet("Message")]
        public IActionResult Message()
        {
            var fileName = "test.txt";
            var content = "This is a test file.";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            IFormFile dummyFile = new FormFile(stream, 0, bytes.Length, "dummyFile", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            var files = new FormFileCollection { dummyFile };

            var message = new Message(
                new[] { "kzaraliev@gmail.com" },
                "Test email with dummy file",
                "This is just a test email with a dummy file attached. - asdasdasd"
            );

            emailService.SendEmailAsync(message);
            return Ok(new { message = "API is working!" });
        }

        [HttpGet("Protected")]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok(new { message = "API is working!" });
        }
    }
}
