namespace Catering.Core.DTOs.Partnership
{
    public class PartnershipRequestListDto
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public IEnumerable<PartnershipItemsDto> Items { get; set; } = new List<PartnershipItemsDto>();
    }
}
