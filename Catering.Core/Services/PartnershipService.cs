using Catering.Core.Contracts;
using Catering.Core.DTOs.Partnership;
using Catering.Core.DTOs.Restaurant;
using Catering.Core.Models.Email;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Enums;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Catering.Core.Services
{
    public class PartnershipService : IPartnershipService
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRestaurantService restaurantService;
        private readonly IEmailService emailService;

        public PartnershipService(IRepository _repository, UserManager<ApplicationUser> _userManager, IRestaurantService _restaurantService, IEmailService _emailService)
        {
            repository = _repository;
            userManager = _userManager;
            restaurantService = _restaurantService;
            emailService = _emailService;
        }

        public async Task ApproveRequestAsync(int requestId)
        {
            var request = await repository
                .All<PartnershipRequest>()
                .FirstOrDefaultAsync(p => p.Id == requestId);

            if (request == null)
            {
                throw new KeyNotFoundException($"Partnership request with Id {requestId} not found.");
            }

            if (request.Status != PartnershipRequestStatus.Pending)
            {
                throw new InvalidOperationException("This request has already been managed.");
            }

            var user = await userManager.FindByEmailAsync(request.ContactEmail);

            var restaurantDto = new CreateRestaurantRequestDto()
            {
                Name = request.RestaurantName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                ContactEmail = request.ContactEmail,
                OwnerId = user?.Id,
            };

            int restaurantId = await restaurantService.CreateRestaurant(restaurantDto);

            //Send email to notify the user when the restourant is approved

            request.Status = PartnershipRequestStatus.Approved;
            request.ApprovedAt = DateTime.UtcNow;
            request.RestaurantId = restaurantId;

            await repository.SaveChangesAsync();
        }

        public async Task<int> SubmitRequestAsync(PartnershipDto dto)
        {
            bool exists = await repository
                .AllReadOnly<PartnershipRequest>()
                .AnyAsync(r => r.ContactEmail == dto.ContactEmail && r.RestaurantName.ToLower() == dto.RestaurantName.ToLower());

            if (exists)
            {
                throw new ValidationException("A request with the same email and restaurant already exists.");
            }

            var request = new PartnershipRequest
            {
                RestaurantName = dto.RestaurantName,
                ContactEmail = dto.ContactEmail,
                PhoneNumber = dto.PhoneNumber,
                Message = dto.Message,
                Address = dto.Address,
            };

            await repository.AddAsync(request);
            await repository.SaveChangesAsync();

            return request.Id;
        }
    }
}
