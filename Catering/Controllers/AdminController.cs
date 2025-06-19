using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleNames.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService _adminService)
        {
            adminService = _adminService;
        }

        [HttpPost("users/{targetUserId}/roles")]
        public async Task<IActionResult> AssignRoleToUser(string targetUserId, AssignUserRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await adminService.AssignRoleToUserAsync(targetUserId, request.RoleName);
            return Ok(new { message = $"Role '{request.RoleName}' assigned to user with ID '{targetUserId}'" });
        }

        [HttpDelete("users/{targetUserId}/roles")]
        public async Task<IActionResult> RemoveRoleFromUser(string targetUserId, RemoveUserRoleDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await adminService.RemoveRoleFromUserAsync(targetUserId, request.RoleName);
            return Ok(new { message = $"Role '{request.RoleName}' removed from user with ID '{targetUserId}'" });
        }
    }
}
