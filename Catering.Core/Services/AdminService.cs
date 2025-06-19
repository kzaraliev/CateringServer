using Catering.Core.Contracts;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Catering.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task AssignRoleToUserAsync(string targetUserId, string roleName)
        {
            var user = await userManager.FindByIdAsync(targetUserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID '{targetUserId}' not found.");
            }

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                throw new KeyNotFoundException($"Role '{roleName}' not found.");
            }

            var isInRole = await userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                throw new InvalidOperationException($"User '{user.UserName}' (ID: {targetUserId}) is already in role '{roleName}'.");
            }

            var result = await userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to assign role '{roleName}' to user '{user.UserName}' (ID: {targetUserId}): {errors}");
            }
        }

        public async Task RemoveRoleFromUserAsync(string targetUserId, string roleName)
        {
            var user = await userManager.FindByIdAsync(targetUserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID '{targetUserId}' not found.");
            }

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                throw new KeyNotFoundException($"Role '{roleName}' not found.");
            }

            var isInRole = await userManager.IsInRoleAsync(user, roleName);
            if (!isInRole)
            {
                throw new InvalidOperationException($"User '{user.UserName}' (ID: {targetUserId}) is not in role '{roleName}'.");
            }

            var result = await userManager.RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to remove role '{roleName}' from user '{user.UserName}' (ID: {targetUserId}): {errors}");
            }
        }
    }
}
