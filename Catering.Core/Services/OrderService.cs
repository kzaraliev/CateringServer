using Catering.Core.Contracts;
using Catering.Core.DTOs.Order;
using Catering.Core.DTOs.OrderItem;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Enums;
using Catering.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repository;
        private readonly ICartService cartService;

        public OrderService(IRepository _repository, ICartService _cartService)
        {
            repository = _repository;
            cartService = _cartService;
        }

        public async Task<OrderDto> PlaceOrderAsync(string? userId, Guid? cartId, PlaceOrderRequestDto request)
        {
            var cart = await GetCartForCheckoutAsync(userId, cartId);

            if (!cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cannot place an order from an empty cart.");
            }

            var distinctRestaurantIds = cart.CartItems
                                .Select(ci => ci.MenuItem?.MenuCategory?.RestaurantId)
                                .Where(id => id.HasValue)
                                .Distinct()
                                .ToList();

            if (distinctRestaurantIds.Count != 1)
            {
                throw new InvalidOperationException("Cart contains items from multiple restaurants or no valid restaurant.");
            }

            int restaurantId = distinctRestaurantIds.Single()!.Value;

            if (request.OrderType == OrderType.Delivery)
            {
                if (string.IsNullOrWhiteSpace(request.Street) ||
                    string.IsNullOrWhiteSpace(request.City) ||
                    string.IsNullOrWhiteSpace(request.PostalCode))
                {
                    throw new InvalidOperationException("Street, City, and Postal Code are required for delivery orders.");
                }
            }
            else // OrderType.Pickup
            {
                request.Street = null;
                request.City = null;
                request.PostalCode = null;
            }

            if (userId == null)
            {
                if (string.IsNullOrWhiteSpace(request.GuestEmail) || string.IsNullOrWhiteSpace(request.GuestPhoneNumber))
                {
                    throw new InvalidOperationException("Guest email and phone number are required for guest orders.");
                }
                if (string.IsNullOrWhiteSpace(request.GuestName))
                {
                    throw new InvalidOperationException("Guest name is required for guest orders.");
                }
            }


            decimal subtotal = cart.CartItems.Sum(ci => ci.Quantity * (ci.MenuItem?.Price ?? 0M));

            // Fetch the restaurant to determine delivery fee.
            var restaurant = await repository.AllReadOnly<Restaurant>()
                                             .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");
            }

            ApplicationUser? user = null;
            if (userId != null)
            {
                user = await repository.All<ApplicationUser>()
                                        .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new KeyNotFoundException($"Authenticated user with ID {userId} not found.");
                }
            }

            decimal deliveryFee = 0M;

            //Will do!
            //if (request.OrderType == OrderType.Delivery)
            //{
            //    deliveryFee = restaurant.DeliveryFee; // Assuming Restaurant has a DeliveryFee property
            //                                          // Implement more complex logic based on distance, order total, etc., here if needed
            //}

            decimal orderTotal = subtotal + deliveryFee;

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                RestaurantId = restaurantId,
                Status = OrderStatus.Pending,
                OrderType = request.OrderType,
                Street = request.Street,
                City = request.City,
                PostalCode = request.PostalCode,
                RequestedDeliveryTime = request.RequestedDeliveryTime,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                OrderTotal = orderTotal,
                PaymentMethod = request.PaymentMethod,
                Notes = request.Notes
            };

            if (userId != null)
            {
                order.CustomerId = userId;

                order.GuestEmail = null;
                order.GuestPhoneNumber = null;
                order.GuestName = null;
            }
            else
            {
                order.CustomerId = null;

                order.GuestEmail = request.GuestEmail;
                order.GuestPhoneNumber = request.GuestPhoneNumber;
                order.GuestName = request.GuestName;
            }

            await repository.AddAsync(order);
            await repository.SaveChangesAsync();

            order.Restaurant = restaurant;
            order.Customer = user;

            var orderItems = MapCartItemsToOrderItems(cart.CartItems);
            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
                await repository.AddAsync(orderItem);
            }

            await repository.SaveChangesAsync();

            await cartService.ClearCartAsync(cartId, userId);

            return MapOrderToDtoAsync(order);
        }

        private async Task<Cart> GetCartForCheckoutAsync(string? userId, Guid? cartId)
        {
            Cart? cart;
            if (userId != null)
            {
                cart = await repository.AllReadOnly<Cart>()
                                       .Include(c => c.CartItems)
                                           .ThenInclude(ci => ci.MenuItem)
                                               .ThenInclude(mi => mi.MenuCategory)
                                       .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            else
            {
                if (!cartId.HasValue)
                {
                    throw new InvalidOperationException("Cart ID is required for guest checkout.");
                }
                cart = await repository.AllReadOnly<Cart>()
                                       .Include(c => c.CartItems)
                                           .ThenInclude(ci => ci.MenuItem)
                                               .ThenInclude(mi => mi.MenuCategory)
                                       .FirstOrDefaultAsync(c => c.Id == cartId.Value && c.UserId == null);
            }

            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found or does not belong to the current user/session.");
            }

            await cartService.PerformCartCleanupAsync(cart);

            if (!cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty or all items are unavailable after cleanup.");
            }

            return cart;
        }

        private ICollection<OrderItem> MapCartItemsToOrderItems(ICollection<CartItem> cartItems)
        {
            return cartItems.Select(ci => new OrderItem
            {
                MenuItemId = ci.MenuItemId,
                ItemName = ci.MenuItem?.Name ?? "Unknown Item",
                ItemImageUrl = ci.MenuItem?.ImageUrl,
                Quantity = ci.Quantity,
                UnitPrice = ci.MenuItem?.Price ?? 0M,
                TotalPrice = ci.Quantity * (ci.MenuItem?.Price ?? 0M)
            }).ToList();
        }

        private OrderDto MapOrderToDtoAsync(Order order)
        {
            var orderItemsDto = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OriginalMenuItemId = oi.MenuItemId,
                ItemName = oi.ItemName,
                ItemImageUrl = oi.ItemImageUrl,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.TotalPrice
            }).ToList();

            string? customerName = null;
            string? customerEmail = null;

            if (order.CustomerId != null)
            {
                if (order.Customer != null)
                {
                    customerName = order.Customer.UserName;
                    customerEmail = order.Customer.Email;
                }
            }

            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                GuestEmail = order.GuestEmail,
                GuestPhoneNumber = order.GuestPhoneNumber,
                GuestName = order.GuestName,
                RestaurantId = order.RestaurantId,
                RestaurantName = order.Restaurant?.Name ?? "Unknown Restaurant",
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderType = order.OrderType,
                Street = order.Street,
                City = order.City,
                PostalCode = order.PostalCode,
                RequestedDeliveryTime = order.RequestedDeliveryTime,
                ActualDeliveryTime = order.ActualDeliveryTime,
                Subtotal = order.Subtotal,
                DeliveryFee = order.DeliveryFee,
                OrderTotal = order.OrderTotal,
                PaymentMethod = order.PaymentMethod,
                Notes = order.Notes,
                Items = orderItemsDto
            };
        }
    }
}
