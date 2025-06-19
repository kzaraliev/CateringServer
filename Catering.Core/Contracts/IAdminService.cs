namespace Catering.Core.Contracts
{
    public interface IAdminService
    {
        Task AssignRoleToUserAsync(string targetUserId, string roleName);
        Task RemoveRoleFromUserAsync(string targetUserId, string roleName);
    }
}
