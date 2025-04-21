using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Identity
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        public required string Token { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public required string RefreshToken { get; set; }
    }
}
