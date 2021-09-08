using System;
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
    public class BookTableController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ICustomerBookingService customerBookingService;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public BookTableController(
            ICustomerService customerService,
            ICustomerBookingService customerBookingService,
            ILoggerManager logger,
            IMapper mapper)
        {
            this.customerService = customerService;
            this.customerBookingService = customerBookingService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Route(API_REQUESTS.BOOK_CUSTOMER_TABLE)]
        [HttpPost]
        public async Task<IActionResult> BookCustomerTable([FromForm] CustomerBookingViewModel customerBookingViewModel)
        {
            try
            {
                var customerExists = await this.customerService.CheckCustomerExistsAsync(customerBookingViewModel.CustomerId).ConfigureAwait(false);
                if (!customerExists)
                    return NotFound(new ApiResponseViewModel
                    {
                        Error = true,
                        Message = API_RESPONSES.CUSTOMER_NOT_FOUND,
                        Data = null
                    });

                var customerBooking = this.mapper.Map<Common.DomainModels.CustomerBooking>(customerBookingViewModel);
                var result = await this.customerBookingService.BookCustomerTableAsync(customerBooking).ConfigureAwait(false);
                return Created(string.Empty, new ApiResponseViewModel
                {
                    Error = false,
                    Message = null,
                    Data = result
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
