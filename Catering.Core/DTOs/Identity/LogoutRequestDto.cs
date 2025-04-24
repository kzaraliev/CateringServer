using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Identity
{
    public class LogoutRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
