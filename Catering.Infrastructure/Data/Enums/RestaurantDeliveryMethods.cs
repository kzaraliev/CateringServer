namespace Catering.Infrastructure.Data.Enums
{
    /// <summary>
    /// Represents the delivery methods that a restaurant can support.
    /// </summary>
    public enum RestaurantDeliveryMethods
    {
        /// <summary>
        /// No delivery methods supported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Customer can pick up their order from the restaurant.
        /// </summary>
        Pickup = 1,

        /// <summary>
        /// Restaurant offers delivery by their own drivers.
        /// </summary>
        RestaurantDelivery = 2,

        /// <summary>
        /// Restaurant supports both pickup and delivery.
        /// </summary>
        All = Pickup | RestaurantDelivery
    }
}
