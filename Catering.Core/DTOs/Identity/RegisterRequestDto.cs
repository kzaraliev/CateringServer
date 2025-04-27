using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Identity
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        public required string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = RequiredMessage)]
        public required string Email { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public required string Password { get; set; }

        public string ClientUri { get; set; } = null!;
    }
}
