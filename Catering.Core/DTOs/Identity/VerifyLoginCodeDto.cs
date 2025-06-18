using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Identity
{
    public class VerifyLoginCodeDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Code { get; set; }
    }
}
