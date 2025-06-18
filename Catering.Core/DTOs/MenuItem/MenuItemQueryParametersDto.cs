using Catering.Core.DTOs.Queries;

namespace Catering.Core.DTOs.MenuItem
{
    public class MenuItemQueryParametersDto : QueryParametersDto
    {
        public bool? IsAvailable { get; set; }
    }
}
