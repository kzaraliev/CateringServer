using System.ComponentModel.DataAnnotations;

namespace Catering.Core.DTOs.Order
{
    public class CancelOrderRequestDto
    {
        [Required]
        public int OrderId { get; set; }

        [EmailAddress]
        public string? GuestEmail { get; set; }
    }
}
