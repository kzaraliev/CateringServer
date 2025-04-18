using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Identity
{
    public class ResetPasswordRequestDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        public required string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = Mismatch)]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public required string Token { get; set; }
    }
}
