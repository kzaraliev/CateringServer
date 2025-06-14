using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.MenuCategoryConstants;

namespace Catering.Core.DTOs.MenuCategory
{
    public class UpdateMenuCategoryDto
    {
        [MinLength(1)]
        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [MinLength(1)]
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
