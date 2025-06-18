namespace Catering.Core.DTOs.MenuItem
{
    public class RelatedMenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
