using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Identity
{
    public class RegisterRequestDto
    {
        [EmailAddress]
        [Required(ErrorMessage = RequiredMessage)]
        public required string Email { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public string ClientUri { get; set; } = null!;
    }
}
