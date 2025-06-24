using Catering.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.AddressConstants;
using static Catering.Infrastructure.Constants.OrderConstants;

namespace Catering.Core.DTOs.Order
{
    public class PlaceOrderRequestDto
    {
        [Required(ErrorMessage = "Order type is required.")]
        public OrderType OrderType { get; set; }

        [StringLength(StreetMaxLength, MinimumLength = 1, ErrorMessage = "Street address must be between {2} and {1} characters long.")]
        public string? Street { get; set; }

        [StringLength(CityMaxLength, MinimumLength = 1, ErrorMessage = "City must be between {2} and {1} characters long.")]
        public string? City { get; set; }

        [StringLength(ZipCodeMaxLength, MinimumLength = 1, ErrorMessage = "Postal code must be between {2} and {1} characters long.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Requested delivery/pickup time is required.")]
        public DateTime RequestedDeliveryTime { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(NotesMaxLength, ErrorMessage = "Notes cannot exceed {1} characters.")]
        public string? Notes { get; set; }

        // For guest orders, these fields are required
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? GuestEmail { get; set; }

        [Phone]
        public string? GuestPhoneNumber { get; set; }

        [StringLength(3, MinimumLength = 2, ErrorMessage = "First name must be between {2} and {1} characters.")]
        public string? GuestName { get; set; }
    }
}
