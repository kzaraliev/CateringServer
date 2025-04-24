using Catering.Core.Contracts;
using Catering.Core.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Catering.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration _config)
        {
            config = _config;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateMessage(message);

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateMessage(Message message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config["Email:From"]));
            email.To.AddRange(message.To);
            email.Subject = message.Subject;
            
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<p>{0}</p>", message.Content)};
            
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream()) 
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            return email;
        }

        private async Task SendAsync(MimeMessage email)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(config["Email:SmtpServer"], 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(config["Email:Username"], config["Email:Password"]);
                    await client.SendAsync(email);
                }
                catch (Exception)
                {
                    throw new Exception("Failed to send email");
                }
                finally 
                {
                    await client.DisconnectAsync(true);
                }                
            }
        }
    }
}
