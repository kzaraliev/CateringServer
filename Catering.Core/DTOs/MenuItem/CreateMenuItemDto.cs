﻿using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.MenuItemConstants;

namespace Catering.Core.DTOs.MenuItem
{
    public class CreateMenuItemDto
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public required string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public int MenuCategoryId { get; set; }
    }
}
