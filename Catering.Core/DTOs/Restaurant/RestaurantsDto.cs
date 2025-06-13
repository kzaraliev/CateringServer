namespace Catering.Core.DTOs.Restaurant
{
    public class RestaurantsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
