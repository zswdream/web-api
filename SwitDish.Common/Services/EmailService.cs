using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SwitDish.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Common.Services
{
    public class EmailService : INotificationService
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerManager logger;
        public EmailService(IConfiguration configuration, ILoggerManager logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        public async Task<bool> SendEmailAsync(string emailAddress, string subject, string message)
        {
            try
            {
                var email = new MailMessage();
                email.To.Add(new MailAddress(emailAddress));
                email.From = new MailAddress(configuration["Email_Service:FromAddress"], configuration["Email_Service:FromName"]);
                email.Subject = subject;

                email.Body = message+ "<br/><hr/><small>This is an automatic email sent from SwitDish</small>";
                email.IsBodyHtml = true;

                using var smtp = new SmtpClient();
                var credential = new NetworkCredential
                {
                    UserName = configuration["Email_Service:UserId"],
                    Password = configuration["Email_Service:UserPassword"]
                };
                smtp.Credentials = credential;
                smtp.Host = configuration["Email_Service:MailServerAddress"];
                smtp.Port = Convert.ToInt32(configuration["Email_Service:MailServerPort"]);
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(email).ConfigureAwait(false);
                return true;
            }
        
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
