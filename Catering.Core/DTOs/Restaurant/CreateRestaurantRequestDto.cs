using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;
using static Catering.Infrastructure.Constants.RestaurantConstants;

namespace Catering.Core.DTOs.Restaurant
{
    public class CreateRestaurantRequestDto
    {
        [MaxLength(NameMaxLength)]
        [Required(ErrorMessage = RequiredMessage)]
        public string Name { get; set; } = null!;

        [EmailAddress]
        [Required(ErrorMessage = RequiredMessage)]
        public string ContactEmail { get; set; } = null!;

        [Phone]
        [Required(ErrorMessage = RequiredMessage)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(AddressMaxLength)]
        [Required(ErrorMessage = RequiredMessage)]
        public string Address { get; set; } = null!;

        public string? OwnerId { get; set; }
    }
}
