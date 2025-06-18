using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Identity
{
    public class RequestLoginCodeDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string ClientUri { get; set; }
    }
}
