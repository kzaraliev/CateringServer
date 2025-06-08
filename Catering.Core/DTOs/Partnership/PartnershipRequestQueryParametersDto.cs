namespace Catering.Core.DTOs.Partnership
{
    public class PartnershipRequestQueryParametersDto
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
