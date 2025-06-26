using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.Order;
using Catering.Core.DTOs.OrderItem;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Enums;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repository;
        private readonly ICartService cartService;
        private readonly UserManager<ApplicationUser> userManager;

        public OrderService(IRepository _repository, ICartService _cartService, UserManager<ApplicationUser> _userManager)
        {
            repository = _repository;
            cartService = _cartService;
            userManager = _userManager;
        }

        public async Task<OrderDto> PlaceOrderAsync(string? userId, Guid? cartId, PlaceOrderRequestDto request)
        {
            if (request.RequestedDeliveryTime <= DateTime.UtcNow.AddMinutes(5))
            {
                throw new InvalidOperationException("Requested delivery/pickup time must be in the future (at least 5 minutes from now).");
            }

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

            if (!await IsRestaurantOpenAsync(restaurant.Id, request.RequestedDeliveryTime))
            {
                throw new InvalidOperationException("The restaurant is closed at the requested delivery/pickup time.");
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

            if (request.OrderType == OrderType.Delivery)
            {
                if (!restaurant.SupportedDeliveryMethods.HasFlag(RestaurantDeliveryMethods.Delivery))
                {
                    throw new InvalidOperationException($"Restaurant '{restaurant.Name}' does not offer delivery.");
                }

                deliveryFee = restaurant.DeliveryFee;
            }
            else if (request.OrderType == OrderType.Pickup)
            {
                if (!restaurant.SupportedDeliveryMethods.HasFlag(RestaurantDeliveryMethods.Pickup))
                {
                    throw new InvalidOperationException($"Restaurant '{restaurant.Name}' does not offer pickup.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Unsupported order type: {request.OrderType}");
            }

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

        public async Task<OrderDto> CancelOrderAsync(int orderId, string? userId, string? guestEmail = null)
        {
            var order = await repository.All<Order>()
                                        .Include(o => o.Restaurant)
                                        .Include(o => o.Customer)
                                        .Include(o => o.OrderItems)
                                        .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            bool isAuthorized = false;

            if (userId != null) // Authenticated user
            {
                var cancellingUser = await userManager.FindByIdAsync(userId);
                if (cancellingUser == null)
                {
                    throw new UnauthorizedAccessException($"Authenticated user with ID {userId} not found.");
                }

                bool isCustomerOfOrder = (order.CustomerId == userId);
                bool isAdmin = await userManager.IsInRoleAsync(cancellingUser, RoleNames.Admin);
                bool isModerator = await userManager.IsInRoleAsync(cancellingUser, RoleNames.Moderator);

                if (isCustomerOfOrder || isAdmin || isModerator)
                {
                    isAuthorized = true;
                }
            }
            else // Guest user (userId is null)
            {
                if (!string.IsNullOrWhiteSpace(guestEmail) &&
                    !string.IsNullOrWhiteSpace(order.GuestEmail) &&
                    order.GuestEmail.Equals(guestEmail, StringComparison.OrdinalIgnoreCase))
                {
                    if (order.CustomerId == null)
                    {
                        isAuthorized = true;
                    }
                }
            }

            if (!isAuthorized)
            {
                throw new UnauthorizedAccessException($"User is not authorized to cancel order {orderId}.");
            }

            if (order.Status == OrderStatus.Delivered ||
                order.Status == OrderStatus.Completed ||
                order.Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException($"Order {orderId} cannot be cancelled as it is in '{order.Status}' status.");
            }

            if (order.Status == OrderStatus.Preparing || order.Status == OrderStatus.OutForDelivery || order.Status == OrderStatus.ReadyForPickup)
            {
                throw new InvalidOperationException($"Order {orderId} cannot be cancelled as it is already being processed by the restaurant (Status: {order.Status}). Please contact customer support.");
            }


            order.Status = OrderStatus.Cancelled;

            await repository.SaveChangesAsync();

            return MapOrderToDtoAsync(order);
        }

        private async Task<bool> IsRestaurantOpenAsync(int restaurantId, DateTime requestedTime)
        {
            var restaurant = await repository.AllReadOnly<Restaurant>()
                                             .Include(r => r.WorkingDays)
                                             .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                return false;
            }

            DayOfWeek requestedDayOfWeek = requestedTime.DayOfWeek;
            TimeSpan requestedTimeOfDay = requestedTime.TimeOfDay;

            var workingDay = restaurant.WorkingDays
                                       .FirstOrDefault(wd => wd.Day == requestedDayOfWeek);

            if (workingDay == null)
            {
                return false;
            }

            if (workingDay.OpenTime <= workingDay.CloseTime)
            {
                return requestedTimeOfDay >= workingDay.OpenTime && requestedTimeOfDay <= workingDay.CloseTime;
            }
            else
            {
                return requestedTimeOfDay >= workingDay.OpenTime || requestedTimeOfDay <= workingDay.CloseTime;
            }
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
                ItemName = ci.MenuItem?.Name ?? "Unknown Item",
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
                ItemName = oi.ItemName,
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
