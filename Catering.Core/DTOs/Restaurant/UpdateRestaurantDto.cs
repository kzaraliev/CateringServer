using Catering.Core.DTOs.WorkingDay;
using Catering.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.RestaurantConstants;

namespace Catering.Core.DTOs.Restaurant
{
    public class UpdateRestaurantDto
    {
        [MinLength(1)]
        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [MinLength(1)]
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [MinLength(1)]
        [MaxLength(AddressMaxLength)]
        public string? Address { get; set; }

        public bool? IsPublic { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DeliveryFee { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [EnumDataType(typeof(RestaurantDeliveryMethods))]
        public RestaurantDeliveryMethods? DeliveryMethod { get; set; }

        public List<WorkingDayDto>? WorkingDays { get; set; }
    }
}