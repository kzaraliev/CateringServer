using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Admin
{
    public class RemoveUserRoleDto
    {
        [Required]
        [StringLength(256, ErrorMessage = "Role name cannot exceed 256 characters.")]
        public string RoleName { get; set; } = null!;
    }
}
