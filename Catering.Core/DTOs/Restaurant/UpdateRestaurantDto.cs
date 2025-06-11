using Catering.Core.DTOs.WorkingDay;
using Catering.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;
using static Catering.Infrastructure.Constants.RestaurantConstants;

namespace Catering.Core.DTOs.Restaurant
{
    public class UpdateRestaurantDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        public int Id { get; set; }

        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [MaxLength(AddressMaxLength)]
        public string? Address { get; set; }

        public bool? IsPublic { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [EnumDataType(typeof(RestaurantDeliveryMethods))]
        public RestaurantDeliveryMethods? DeliveryMethod { get; set; }

        public List<WorkingDayDto>? WorkingDays { get; set; }
    }
}