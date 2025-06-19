using Catering.Core.Contracts;
using Catering.Core.DTOs.Admin;
using Catering.Core.DTOs.Queries;
using Catering.Core.Utils;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepository repository;

        public AdminService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IRepository _repository)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            repository = _repository;
        }

        public async Task<List<string?>> GetAllRolesAsync()
        {
            List<string?> response = await roleManager.Roles.Select(r => r.Name).ToListAsync();

            return response;
        }

        public async Task<PagedResult<UserDto>> GetAllUsersAsync(UserQueryParametersDto queryParams)
        {
            IQueryable<ApplicationUser> query = repository.AllReadOnly<ApplicationUser>();

            if (!string.IsNullOrEmpty(queryParams.Role))
            {
                string roleName = queryParams.Role.Trim();

                var userIdsInRole = await (
                    from userRole in repository.AllReadOnly<IdentityUserRole<string>>()
                    join role in repository.AllReadOnly<IdentityRole>() on userRole.RoleId equals role.Id
                    where role.Name == roleName
                    select userRole.UserId
                ).ToListAsync();

                query = query.Where(u => userIdsInRole.Contains(u.Id));
            }

            //Search
            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                string term = queryParams.SearchTerm.Trim().ToLower();

                query = query.Where(u =>
                    (u.UserName != null && u.UserName.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.Id != null && u.Id.ToLower().Contains(term))
                );
            }

            query = query.ApplySorting(queryParams.SortBy, queryParams.SortDescending);

            var response = await query.ToPagedResultAsync(
                queryParams.Page,
                queryParams.PageSize,
                u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                });

            return response;
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
