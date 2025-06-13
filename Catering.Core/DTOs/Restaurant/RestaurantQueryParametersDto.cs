using Catering.Core.DTOs.Queries;
using Catering.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Restaurant
{
    public class RestaurantQueryParametersDto : QueryParametersDto
    {
        public bool? IsOpen { get; set; }

        [EnumDataType(typeof(RestaurantDeliveryMethods))]
        public RestaurantDeliveryMethods? DeliveryMethods { get; set; }
    }
}
