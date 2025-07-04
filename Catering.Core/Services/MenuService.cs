﻿using Catering.Core.Contracts;
using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Core.DTOs.Queries;
using Catering.Core.Utils;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository repository;

        public MenuService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<MenuItemDto> GetMenuItemAsync(int restaurantId, int menuItemId)
        {
            var menuItem = await repository.AllReadOnly<MenuItem>()
                .Include(mi => mi.MenuCategory)
                .FirstOrDefaultAsync(mi => mi.Id == menuItemId && mi.MenuCategory.RestaurantId == restaurantId);

            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {menuItemId} not found for restaurant with ID {restaurantId}.");
            }

            var relatedMenuItems = await repository.AllReadOnly<MenuItem>()
                .Where(mi => mi.MenuCategory.RestaurantId == restaurantId && mi.Id != menuItemId)
                .OrderBy(mi => Guid.NewGuid())
                .Take(5)
                .Select(mi => new RelatedMenuItemDto
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    ImageUrl = mi.ImageUrl,
                    Price = mi.Price,
                    IsAvailable = mi.IsAvailable
                })
                .ToListAsync();

            var menuItemDto = new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                ImageUrl = menuItem.ImageUrl,
                MenuCategoryId = menuItem.MenuCategoryId,
                MenuCategoryName = menuItem.MenuCategory?.Name,
                RelatedMenuItems = relatedMenuItems
            };

            return menuItemDto;
        }

        public async Task<PagedResult<MenuItemsDto>> GetAllMenuItemsForRestaurantAsync(int restaurantId, MenuItemQueryParametersDto queryParams)
        {
            IQueryable<MenuItem> query = repository.AllReadOnly<MenuItem>()
                .Include(mi => mi.MenuCategory);

            query = query.Where(mi => mi.MenuCategory.RestaurantId == restaurantId);

            // Filter by availability if specified
            if (queryParams.IsAvailable.HasValue)
            {
                query = query.Where(mi => mi.IsAvailable == queryParams.IsAvailable.Value);
            }

            //Search
            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                string term = queryParams.SearchTerm.Trim().ToLower();

                query = query.Where(mi =>
                    (mi.Description != null && mi.Description.ToLower().Contains(term)) || 
                    mi.Name.ToLower().Contains(term) ||                                  
                    mi.MenuCategory.Name.ToLower().Contains(term)                        
                );
            }

            query = query.ApplySorting(queryParams.SortBy, queryParams.SortDescending);

            var response = await query.ToPagedResultAsync(
                queryParams.Page,
                queryParams.PageSize,
                mi => new MenuItemsDto
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    Description = mi.Description,
                    ImageUrl = mi.ImageUrl,
                    MenuCategoryId = mi.MenuCategoryId,
                    Price = mi.Price,
                    IsAvailable = mi.IsAvailable,
                });

            return response;
        }

        public async Task CreateMenuItemAsync(int restaurantId, CreateMenuItemDto menuItemDto, string userId)
        {
            await ValidateOwnership(userId, restaurantId);

            var menuCategory = await repository.AllReadOnly<MenuCategory>()
                .FirstOrDefaultAsync(mc => mc.Id == menuItemDto.MenuCategoryId && mc.RestaurantId == restaurantId)
                ?? throw new KeyNotFoundException($"MenuCategory with ID {menuItemDto.MenuCategoryId} not found or does not belong to Restaurant ID {restaurantId}.");

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price,
                IsAvailable = menuItemDto.IsAvailable,
                ImageUrl = menuItemDto.ImageUrl,
                MenuCategoryId = menuItemDto.MenuCategoryId
            };

            await repository.AddAsync(menuItem);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(int restaurantId, int menuItemId, UpdateMenuItemDto menuItemDto, string userId)
        {
            await ValidateOwnership(userId, restaurantId);

            var menuItem = await repository.All<MenuItem>()
                .Include(mi => mi.MenuCategory)
                .FirstOrDefaultAsync(mi => mi.Id == menuItemId && mi.MenuCategory.RestaurantId == restaurantId)
                ?? throw new KeyNotFoundException($"MenuItem with ID {menuItemId} not found for Restaurant ID {restaurantId}.");

            menuItem.Name = menuItemDto.Name ?? menuItem.Name;
            menuItem.Description = menuItemDto.Description ?? menuItem.Description;
            menuItem.Price = menuItemDto.Price ?? menuItem.Price;
            menuItem.IsAvailable = menuItemDto.IsAvailable ?? menuItem.IsAvailable;
            menuItem.ImageUrl = menuItemDto.ImageUrl ?? menuItem.ImageUrl;

            if (menuItemDto.MenuCategoryId.HasValue && menuItemDto.MenuCategoryId.Value != menuItem.MenuCategoryId)
            {
                int newMenuCategoryId = menuItemDto.MenuCategoryId.Value;

                var targetMenuCategory = await repository.AllReadOnly<MenuCategory>()
                    .FirstOrDefaultAsync(mc => mc.Id == newMenuCategoryId && mc.RestaurantId == restaurantId);

                if (targetMenuCategory == null)
                {
                    throw new KeyNotFoundException($"MenuCategory with ID {newMenuCategoryId} not found or does not belong to the current restaurant.");
                }

                menuItem.MenuCategoryId = newMenuCategoryId;
            }

            await repository.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int restaurantId, int menuItemId, string userId)
        {
            await ValidateOwnership(userId, restaurantId);

            var menuItem = await repository.All<MenuItem>()
                .Include(mi => mi.MenuCategory)
                .FirstOrDefaultAsync(mi => mi.Id == menuItemId && mi.MenuCategory.RestaurantId == restaurantId)
                ?? throw new KeyNotFoundException($"MenuItem with ID {menuItemId} not found for Restaurant ID {restaurantId}.");

            repository.Remove(menuItem);
            await repository.SaveChangesAsync();
        }

        public async Task<PagedResult<MenuCategoriesDto>> GetAllMenuCategoriesForRestaurantAsync(int restaurantId, MenuCategoryQueryParametersDto queryParams)
        {
            IQueryable<MenuCategory> query = repository.AllReadOnly<MenuCategory>()
                .Include(mc => mc.MenuItems);

            query = query.Where(mi => mi.RestaurantId == restaurantId);

            //Search
            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                string term = queryParams.SearchTerm.Trim().ToLower();

                query = query.Where(mc =>
                    (mc.Description != null && mc.Description.ToLower().Contains(term)) ||
                    mc.Name.ToLower().Contains(term)
                );
            }

            query = query.ApplySorting(queryParams.SortBy, queryParams.SortDescending);

            var response = await query.ToPagedResultAsync(
                queryParams.Page,
                queryParams.PageSize,
                mc => new MenuCategoriesDto
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Description = mc.Description,
                    RestaurantId = mc.RestaurantId,
                    MenuItemsCount = mc.MenuItems.Count()
                });

            return response;
        }

        public async Task CreateMenuCategoryAsync(int restaurantId, CreateMenuCategoryDto menuCategoryDto, string userId)
        {
            await ValidateOwnership(userId, restaurantId);

            var menuCategory = new MenuCategory
            {
                Name = menuCategoryDto.Name,
                Description = menuCategoryDto.Description,
                RestaurantId = restaurantId
            };
            await repository.AddAsync(menuCategory);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateMenuCategoryAsync(int restaurantId, int menuCategoryId, UpdateMenuCategoryDto menuCategoryDto, string userId)
        {
            await ValidateOwnership(userId, restaurantId);

            var menuCategory = await repository.All<MenuCategory>()
                 .FirstOrDefaultAsync(mc => mc.Id == menuCategoryId && mc.RestaurantId == restaurantId)
                 ?? throw new KeyNotFoundException($"MenuCategory with ID {menuCategoryId} not found for Restaurant ID {restaurantId}.");

            menuCategory.Name = menuCategoryDto.Name ?? menuCategory.Name;
            menuCategory.Description = menuCategoryDto.Description ?? menuCategory.Description;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteMenuCategoryAsync(int restaurantId, int menuCategoryId, string userId)
        {
            await ValidateOwnership(userId, restaurantId);

            var menuCategory = await repository.All<MenuCategory>()
                .Include(mc => mc.MenuItems)
                .FirstOrDefaultAsync(mc => mc.Id == menuCategoryId && mc.RestaurantId == restaurantId)
                ?? throw new KeyNotFoundException($"MenuCategory with ID {menuCategoryId} not found for Restaurant ID {restaurantId}.");

            if (menuCategory.MenuItems.Any())
            {
                throw new InvalidOperationException("Cannot delete a menu category that contains menu items. Please remove the items first.");
            }

            repository.Remove(menuCategory);
            await repository.SaveChangesAsync();
        }

        public async Task CreateDefaultMenuCategoryAsync(int restaurantId)
        {

            var menuCategory = new MenuCategory
            {
                Name = "Без категория",
                RestaurantId = restaurantId
            };

            await repository.AddAsync(menuCategory);
            await repository.SaveChangesAsync();
        }

        private async Task ValidateOwnership(string userId, int restaurantId)
        {
            var restaurant = await repository.AllReadOnly<Restaurant>()
                .FirstOrDefaultAsync(r => r.Id == restaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");

            if (restaurant.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action on the restaurant.");
            }
        }
    }
}
