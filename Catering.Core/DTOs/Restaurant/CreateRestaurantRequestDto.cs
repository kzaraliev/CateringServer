using Catering.Core.DTOs.WorkingDay;
using Catering.Infrastructure.Data.Models;
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

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = RequiredMessage)]
        public string ContactEmail { get; set; } = null!;

        [Phone]
        [Required(ErrorMessage = RequiredMessage)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(AddressMaxLength)]
        [Required(ErrorMessage = RequiredMessage)]
        public string Address { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [EnumDataType(typeof(RestaurantDeliveryMethods))]
        public RestaurantDeliveryMethods SupportedDeliveryMethods { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public string OwnerId { get; set; } = null!;

        public List<WorkingDayDto> WorkingDays { get; set; } = new();
    }
}
