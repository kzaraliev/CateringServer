using Catering.Core.DTOs.Queries;

namespace Catering.Core.DTOs.Admin
{
    public class UserQueryParametersDto : QueryParametersDto
    {
        public string? Role { get; set; }
    }
}
