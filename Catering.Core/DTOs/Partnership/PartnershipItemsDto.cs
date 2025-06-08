using Catering.Infrastructure.Data.Enums;

namespace Catering.Core.DTOs.Partnership
{
    public class PartnershipItemsDto
    {
        public int Id { get; set; }
        public required string RestaurantName { get; set; }
        public required string ContactEmail { get; set; }
        public required string PhoneNumber { get; set; }
        public PartnershipRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? RestaurantId { get; set; }
        public string Address { get; set; } = null!;

    }
}