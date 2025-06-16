using Catering.Core.Contracts;
using Catering.Core.DTOs.Cart;
using Catering.Core.DTOs.CartItem;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository repository;

        public CartService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<CartDto> GetOrCreateCartAsync(Guid? cartId, string? userId)
        {
            var cart = await GetOrCreateCartEntityAsync(cartId, userId);

            var response = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Subtotal = cart.CartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price),
                TotalItems = cart.CartItems.Sum(ci => ci.Quantity),
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    Price = ci.MenuItem.Price,
                    Name = ci.MenuItem.Name,
                    ImageUrl = ci.MenuItem.ImageUrl,
                }).ToList()
            };

            return response;
        }

        public async Task<CartDto> AddItemToCartAsync(Guid? cartId, string? userId, AddItemToCartRequestDto request)
        {
            var cart = await GetOrCreateCartEntityAsync(cartId, userId);

            var menuItem = await repository.AllReadOnly<MenuItem>()
                                           .FirstOrDefaultAsync(m => m.Id == request.MenuItemId);

            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {request.MenuItemId} not found or not available.");
            }

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.MenuItemId == request.MenuItemId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += request.Quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    MenuItemId = request.MenuItemId,
                    Quantity = request.Quantity,
                    MenuItem = menuItem
                };
                cart.CartItems.Add(newCartItem);
            }

            cart.LastModified = DateTime.UtcNow;

            await repository.SaveChangesAsync();

            var response = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Subtotal = cart.CartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price),
                TotalItems = cart.CartItems.Sum(ci => ci.Quantity),
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    Price = ci.MenuItem.Price,
                    Name = ci.MenuItem.Name,
                    ImageUrl = ci.MenuItem.ImageUrl,
                }).ToList()
            };

            return response;
        }

        private async Task<Cart> GetOrCreateCartEntityAsync(Guid? cartId, string? userId)
        {
            Cart? cart = null;

            // --- Authenticated User Logic ---
            if (userId != null)
            {
                cart = await repository.All<Cart>()
                                         .Include(c => c.CartItems)
                                         .ThenInclude(ci => ci.MenuItem)
                                         .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow
                    };
                    await repository.AddAsync(cart);
                }
            }
            // --- Guest User Logic ---
            else
            {
                if (cartId.HasValue)
                {
                    cart = await repository.All<Cart>()
                                         .Include(c => c.CartItems)
                                         .ThenInclude(ci => ci.MenuItem)
                                         .FirstOrDefaultAsync(c => c.Id == cartId.Value && c.UserId == null);
                }

                if (cart == null)
                {
                    cart = new Cart
                    {
                        Id = Guid.NewGuid(),
                        UserId = null,
                        CreatedAt = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow
                    };
                    await repository.AddAsync(cart);
                }
            }

            await repository.SaveChangesAsync();

            return cart;
        }

    }
}
