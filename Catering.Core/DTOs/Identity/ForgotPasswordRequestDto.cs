using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Identity
{
    public class ForgotPasswordRequestDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public required string ClientUri { get; set; }
    }
}
