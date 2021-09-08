using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using SwitDish.Customer.Services.Interfaces;

namespace SwitDish.Customer.API.Controllers.CustomerControllers
{
    [ApiController]
    public class GetFavouriteVendorsController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerFavouriteVendorService customerFavouriteVendorService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public GetFavouriteVendorsController(
            ICustomerService customerService,
            ICustomerFavouriteVendorService customerFavouriteVendorService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.customerFavouriteVendorService = customerFavouriteVendorService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.GET_FAVOURITE_VENDORS)]
        [HttpGet]
        public async Task<IActionResult> GetFavouriteVendors([FromQuery] int customerId)
        {
            try
            {
                var customerExists = await this.customerService.CheckCustomerExistsAsync(customerId).ConfigureAwait(false);
                if (!customerExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_NOT_FOUND,
                        Data = null
                    });

                var favourites = await this.customerFavouriteVendorService.GetCustomerFavouriteVendorsAsync(customerId).ConfigureAwait(false);

                if (favourites.Count() > 0)
                    return Ok(new ApiResponseViewModel
                    {
                        Error = false,
                        Message = null,
                        Data = this.mapper.Map<IEnumerable<CustomerFavouriteVendorViewModel>>(favourites)
                    });

                return Ok(new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = new object[] { }
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
