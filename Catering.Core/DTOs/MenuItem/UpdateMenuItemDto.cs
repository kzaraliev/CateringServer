using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.MenuItemConstants;

namespace Catering.Core.DTOs.MenuItem
{
    public class UpdateMenuItemDto
    {
        [MinLength(1)]
        [MaxLength(NameMaxLength)]
        public string? Name { get; set; }

        [MinLength(1)]
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? Price { get; set; }

        public bool? IsAvailable { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public int? MenuCategoryId { get; set; }
    }
}
