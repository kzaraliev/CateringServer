namespace Catering.Core.DTOs.MenuItem
{
    public class MenuItemsDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public required int MenuCategoryId { get; set; }
    }
}
