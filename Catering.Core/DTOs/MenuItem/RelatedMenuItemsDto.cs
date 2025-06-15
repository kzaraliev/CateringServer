namespace Catering.Core.DTOs.MenuItem
{
    public class RelatedMenuItemsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
