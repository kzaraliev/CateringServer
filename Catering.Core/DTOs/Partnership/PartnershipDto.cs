using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;
using static Catering.Infrastructure.Constants.RequestConstants;

namespace Catering.Core.DTOs.Partnership
{
    public class PartnershipDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(RestaurantNameMaxLength)]
        public required string RestaurantName { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [EmailAddress]
        public required string ContactEmail { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Phone]
        public required string PhoneNumber { get; set; }

        [MaxLength(AddressMaxLength)]
        [Required(ErrorMessage = RequiredMessage)]
        public string Address { get; set; } = null!;

        [StringLength(MessageMaxLength)]
        public string Message { get; set; } = string.Empty;
    }
}
