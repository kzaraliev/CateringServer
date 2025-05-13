using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Identity
{
    public class ConfirmEmailRequestDto
    {
        [EmailAddress]
        [Required(ErrorMessage = RequiredMessage)]
        public required string Email { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public required string Token { get; set; }
    }
}
