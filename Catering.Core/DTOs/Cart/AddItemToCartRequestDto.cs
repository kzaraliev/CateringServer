using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Cart
{
    public class AddItemToCartRequestDto
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
