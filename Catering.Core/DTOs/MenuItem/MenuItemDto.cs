namespace Catering.Core.DTOs.MenuItem
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int MenuCategoryId { get; set; }
        public string? MenuCategoryName { get; set; }
        public IEnumerable<RelatedMenuItemsDto> RelatedMenuItems { get; set; } = new List<RelatedMenuItemsDto>();
    }
}
