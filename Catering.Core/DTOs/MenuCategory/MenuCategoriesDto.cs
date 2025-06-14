namespace Catering.Core.DTOs.MenuCategory
{
    public class MenuCategoriesDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required int RestaurantId { get; set; }
        public int MenuItemsCount { get; set; }
    }
}
