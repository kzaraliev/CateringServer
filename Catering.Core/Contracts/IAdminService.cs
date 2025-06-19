using Catering.Core.DTOs.Admin;
using Catering.Core.DTOs.Queries;

namespace Catering.Core.Contracts
{
    public interface IAdminService
    {
        Task AssignRoleToUserAsync(string targetUserId, string roleName);
        Task RemoveRoleFromUserAsync(string targetUserId, string roleName);
        Task<PagedResult<UserDto>> GetAllUsersAsync(UserQueryParametersDto queryParams);
    }
}
