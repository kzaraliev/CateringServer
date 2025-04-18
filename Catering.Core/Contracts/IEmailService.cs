using Catering.Core.Models.Email;

namespace Catering.Core.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
