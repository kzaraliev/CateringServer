namespace Catering.Core.DTOs.MenuItem
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public int MenuCategoryId { get; set; }
        public string? MenuCategoryName { get; set; }
        public IEnumerable<RelatedMenuItemDto> RelatedMenuItems { get; set; } = new List<RelatedMenuItemDto>();
    }
}
