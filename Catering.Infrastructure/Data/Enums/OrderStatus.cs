namespace Catering.Infrastructure.Data.Enums
{
    public enum OrderStatus
    {
        Pending,           // New order, awaiting restaurant review
        Approved,          // Restaurant accepted the order
        Denied,            // Restaurant denied the order
        Preparing,         // Food is being prepared
        OutForDelivery,    // Order is on its way (if delivery)
        ReadyForPickup,    // Order is ready for customer pickup (if pickup)
        Delivered,         // Order successfully delivered/picked up
        CancelledByUser,   // User cancelled before approval/preparation
        CancelledByRestaurant // Restaurant cancelled after approval (e.g., unexpected issues)
    }

}
