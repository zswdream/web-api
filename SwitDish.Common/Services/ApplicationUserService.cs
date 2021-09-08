using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SwitDish.Common.Interfaces;
using SwitDish.Common.ViewModels;
using SwitDish.DataModel.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Common.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository<ApplicationUser> applicationUserRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoggerManager logger;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        public ApplicationUserService(
            IRepository<ApplicationUser> applicationUserRepository,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerManager logger,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        public async Task<Common.DomainModels.ApplicationUser> GetUser(int applicationUserId)
        {
            try
            {
                var user = await this.applicationUserRepository.GetAsync(applicationUserId).ConfigureAwait(false);
                if(user != null)
                return this.mapper.Map<Common.DomainModels.ApplicationUser>(user);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<Common.DomainModels.ApplicationUser> FindUserByCredentials(string email, string password)
        {
            try
            {
                var query = await applicationUserRepository.GetAsync(filter: d => d.Email.Equals(email.ToLower())).ConfigureAwait(false);
                var user = query.FirstOrDefault();
            
                if(user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return this.mapper.Map<Common.DomainModels.ApplicationUser>(user);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
            }   
            return null;
        }
        public async Task<string> SendPasswordResetEmail(string emailAddress)
        {
            try
            {
                var query = await this.applicationUserRepository.GetAsync(filter: d=> d.Email == emailAddress).ConfigureAwait(false);
                var user = query.SingleOrDefault();

                if(user != null)
                {
                    // Generate randow 6-digit code
                    var randomCode = new System.Random().Next(999, 999999).ToString().PadLeft(6, '0');
                    // Save code in session against user email
                    httpContextAccessor.HttpContext.Session.SetString(user.Email, randomCode);
                    // Create email for password reset
                    var emailSubject = "SwitDish Password Reset";
                    var emailMessage = $"<h1>Password Reset</h1><h3>Password reset code for <strong>{user.Email}</strong> is <strong><em>{randomCode}</em></strong>.<p>This code is only valid for {configuration.GetValue<int>("SessionTimeout")} minutes.</p>";

                    // Send email to user
                    var emailSent = await this.notificationService.SendEmailAsync(user.Email, emailSubject, emailMessage).ConfigureAwait(false);
                    if(emailSent)
                        return "EMAIL_SENT";

                    return "EMAIL_SENDING_ERROR";
                }
                return "USER_NOT_FOUND";
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return "SERVER_ERROR";
            }
        }
        public async Task<string> ResetPassword(PasswordResetViewModel data)
        {
            try
            {
                var query = await this.applicationUserRepository.GetAsync(filter: d => d.Email == data.Email).ConfigureAwait(false);
                var user = query.SingleOrDefault();

                if(user != null)
                {
                    if(this.httpContextAccessor.HttpContext.Session.Keys.Contains(user.Email) 
                        && this.httpContextAccessor.HttpContext.Session.GetString(user.Email).Equals(data.VerificationCode))
                    {
                        // Update password hash in database 
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(data.NewPassword);
                        this.applicationUserRepository.Update(user);
                        await this.applicationUserRepository.SaveChangesAsync().ConfigureAwait(false);

                        // Remove code from session
                        this.httpContextAccessor.HttpContext.Session.Remove(user.Email);
                        return "PASSWORD_RESET";
                    }
                    return "WRONG_CODE";
                }
                return "USER_NOT_FOUND";
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return "SERVER_ERROR";
            }
        }
        public async Task<string> UpdatePassword(PasswordUpdateViewModel data)
        {
            try
            {
                var user = await this.applicationUserRepository.GetAsync(data.ApplicationUserId).ConfigureAwait(false);

                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(data.OldPassword, user.PasswordHash))
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(data.NewPassword);
                        this.applicationUserRepository.Update(user);
                        await this.applicationUserRepository.SaveChangesAsync().ConfigureAwait(false);
                        return "PASSWORD_UPDATED";
                    }
                    return "INCORRECT_PASSWORD";
                }
                return "USER_NOT_FOUND";
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return "ERROR";
            }
        }
    }
}
