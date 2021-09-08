using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SwitDish.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace SwitDish.Common.Services
{
    public class SendGridEmailService : INotificationService
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerManager logger;
        
        public SendGridEmailService(IConfiguration configuration, ILoggerManager logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<bool> SendEmailAsync(string emailAddress, string subject, string message)
        {
            try
            {
                var apiKey = configuration["SendGridEmail_Service:API_KEY"];
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(configuration["SendGridEmail_Service:FromAddress"], configuration["SendGridEmail_Service:FromName"]),
                    Subject = subject,
                    PlainTextContent = message,
                    HtmlContent = message
                };
                msg.AddTo(new EmailAddress(emailAddress));
                
                // Disable Tracking by Default
                msg.SetClickTracking(false, false);
                msg.SetOpenTracking(false);
                msg.SetGoogleAnalytics(false);
                msg.SetSubscriptionTracking(false);

                var response = await client.SendEmailAsync(msg);
                if (response.IsSuccessStatusCode)
                    return true;

                return false;
            }
        
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
