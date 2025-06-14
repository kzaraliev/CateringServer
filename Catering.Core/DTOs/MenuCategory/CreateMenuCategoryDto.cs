using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.MenuCategoryConstants;

namespace Catering.Core.DTOs.MenuCategory
{
    public class CreateMenuCategoryDto
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public required string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
