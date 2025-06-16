using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Cart
{
    public class UpdateCartItemQuantityRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int Quantity { get; set; }
    }
}
