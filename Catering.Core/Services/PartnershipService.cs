using Catering.Core.Contracts;
using Catering.Core.DTOs.Partnership;
using Catering.Core.DTOs.Restaurant;
using Catering.Core.Models.Email;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Enums;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public async Task ProcessRequestAsync(ManagePartnershipDto manageRequestDto)
        {
            var request = await repository
                .All<PartnershipRequest>()
                .FirstOrDefaultAsync(p => p.Id == manageRequestDto.PartnershipRequestId);

            if (request == null)
            {
                throw new KeyNotFoundException($"Partnership request with Id {manageRequestDto.PartnershipRequestId} not found.");
            }

            if (request.Status != PartnershipRequestStatus.Pending)
            {
                throw new InvalidOperationException("This request has already been managed.");
            }

            if (manageRequestDto.isApproved)
            {
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

                request.Status = PartnershipRequestStatus.Approved;
                request.RestaurantId = restaurantId;
            }
            else 
            {
                request.Status = PartnershipRequestStatus.Rejected;
            }

            request.ProcessedAt = DateTime.UtcNow;
            await repository.SaveChangesAsync();

            await SendProcessedRequestInfo(request.ContactEmail, manageRequestDto.isApproved);
        }

        public async Task<int> SubmitRequestAsync(PartnershipDto partnershipDto)
        {
            bool exists = await repository
                .AllReadOnly<PartnershipRequest>()
                .AnyAsync(r => r.ContactEmail == partnershipDto.ContactEmail && r.RestaurantName.ToLower() == partnershipDto.RestaurantName.ToLower());

            if (exists)
            {
                throw new ValidationException("A request with the same email and restaurant already exists.");
            }

            var request = new PartnershipRequest
            {
                RestaurantName = partnershipDto.RestaurantName,
                ContactEmail = partnershipDto.ContactEmail,
                PhoneNumber = partnershipDto.PhoneNumber,
                Message = partnershipDto.Message,
                Address = partnershipDto.Address,
            };

            await repository.AddAsync(request);
            await repository.SaveChangesAsync();

            await SendInitialRequestConfirmationEmail(request.ContactEmail);

            return request.Id;
        }

        private async Task SendProcessedRequestInfo(string email, bool isApproved)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hello,");
            builder.AppendLine();
            builder.AppendLine("We wanted to let you know that your request has been reviewed.");

            if (isApproved)
            {
                builder.AppendLine("✅ Status: Approved");
                builder.AppendLine("Congratulations! Your request has been approved.");
            }
            else
            {
                builder.AppendLine("❌ Status: Rejected");
                builder.AppendLine("Unfortunately, your request has not been approved at this time.");
            }

            builder.AppendLine();
            builder.AppendLine("If you have any questions, feel free to contact us.");
            builder.AppendLine();
            builder.AppendLine("Best regards,");
            builder.AppendLine("Ketaring.bg");

            var message = new Message([email], "Your Request Has Been Reviewed", builder.ToString());
            await emailService.SendEmailAsync(message);
        }

        private async Task SendInitialRequestConfirmationEmail(string email)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hello,");
            builder.AppendLine();
            builder.AppendLine("Thank you for submitting your partnership request to Ketaring.bg.");
            builder.AppendLine("We have received your information and our team is currently reviewing your request.");
            builder.AppendLine("You will be notified by email once a decision has been made.");
            builder.AppendLine();
            builder.AppendLine("Best regards,");
            builder.AppendLine("Ketaring.bg");

            var message = new Message([email], "We Received Your Partnership Request", builder.ToString());
            await emailService.SendEmailAsync(message);
        }
    }
}
