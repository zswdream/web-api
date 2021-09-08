using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SwitDish.Common.Authorization;
using SwitDish.Common.Interfaces;
using SwitDish.Common.ViewModels;
using SwitDish.Customer.API.Utilities;

namespace SwitDish.Customer.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IApplicationUserService userService;
        private readonly ILoggerManager logger;

        public AuthController(
            IConfiguration configuration,
            IApplicationUserService authService,
            ILoggerManager logger)
        {
            this.configuration = configuration;
            this.userService = authService;
            this.logger = logger;
        }

        [Route(API_REQUESTS.LOGIN)]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var user = await userService.FindUserByCredentials(username: formData.Email, password: formData.Password).ConfigureAwait(false);
                if (user != null)
                {
                    var tokenString = TokenGenerator.GenerateJSONWebToken(
                        user: user,
                        secretKey: configuration["Jwt_Auth:SecretKey"],
                        issuer: configuration["Jwt_Auth:Issuer"],
                        audience: configuration["Jwt_Auth:Issuer"],
                        tokenValidity: Convert.ToDouble(configuration["Jwt_Auth:TokenValidity"])
                        );

                    return Ok(new { access_token = tokenString, user });
                }

                return Unauthorized();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.REQUEST_PASSWORD_RESET)]
        [HttpGet]
        public async Task<IActionResult> RequestPasswordReset([FromQuery] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            try
            {
                var result = await this.userService.SendPasswordResetEmail(email).ConfigureAwait(false);
                return result switch
                {
                    "SERVER_ERROR" => Problem(result),
                    "EMAIL_SENDING_ERROR" => Problem(result),
                    "USER_NOT_FOUND" => BadRequest(result),
                    _ => Ok(result)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.RESET_PASSWORD)]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] PasswordResetViewModel formData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!formData.NewPassword.Equals(formData.ConfirmPassword))
                return BadRequest(API_RESPONSES.NEW_PASSWORDS_DONT_MATCH);
            try
            {
                var result = await this.userService.ResetPassword(formData).ConfigureAwait(false);
                return result switch
                {
                    "USER_NOT_FOUND" => BadRequest(result),
                    "WRONG_CODE" => BadRequest(result),
                    "SERVER_ERROR" => Problem(result),
                    _ => Ok(result)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [Route(API_REQUESTS.UPDATE_PASSWORD)]
        [HttpPatch]
        public async Task<IActionResult> UpdatePassword([FromForm] PasswordUpdateViewModel formData)
        {
            var customer = await this.userService.GetUser(formData.ApplicationUserId).ConfigureAwait(false);
            if (customer == null)
                return NotFound(API_RESPONSES.USER_NOT_FOUND);

            if (formData.NewPassword.Equals(formData.OldPassword))
                return BadRequest(API_RESPONSES.OLD_AND_NEW_PASSWORDS_SAME);

            try
            {
                var result = await this.userService.UpdatePassword(formData).ConfigureAwait(false);
                return result switch
                {
                    "USER_NOT_FOUND" => BadRequest(API_RESPONSES.USER_NOT_FOUND),
                    "INCORRECT_PASSWORD" => BadRequest(API_RESPONSES.OLD_PASSWORD_INVALID),
                    "PASSWORD_UPDATED" => Ok(result),
                    _ => Problem(result),
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
