namespace Catering.Infrastructure.Data.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Preparing = 2,
        ReadyForPickup = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Completed = 6,
        Cancelled = 7
    }

}
