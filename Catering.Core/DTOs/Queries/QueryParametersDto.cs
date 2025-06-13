namespace Catering.Core.DTOs.Queries
{
    public class QueryParametersDto
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
