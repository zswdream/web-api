using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class UpdateProfileImageController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILoggerManager logger;

        public UpdateProfileImageController(
            ICustomerService customerService,
            ILoggerManager logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        [Route(API_REQUESTS.UPDATE_CUSTOMER_PROFILE_IMAGE)]
        [HttpPatch]
        public async Task<IActionResult> UpdateProfileImage([FromForm] IFormFile imageFile, [FromForm] int customerId)
        {
            try
            {
                var validationResult = FormFileValidator.IsValid(imageFile, new string[] { ".jpg", ".png", ".jpeg" }, 5 * 1024 * 1024);
                if (!String.Equals("VALID", validationResult))
                    return BadRequest(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = null,
                        Data = validationResult
                    });

                var result = await this.customerService.UpdateCustomerProfileImageAsync(customerId, imageFile).ConfigureAwait(false);

                return result switch
                {
                    "CUSTOMER_NOT_FOUND" => NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = null,
                        Data = result
                    }),
                    "ERROR" => Problem(),
                    _ => Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = result
                    })
                };
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
