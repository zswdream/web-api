using AutoMapper;
using Microsoft.Extensions.Configuration;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly IRepository<CustomerOrder> customerOrderRepository;
        private readonly IProductService productService;
        private readonly IVendorOfferService vendorOfferService;
        private readonly ICustomerDeliveryAddressService customerDeliveryAddressService;
        private readonly ICustomerService customerService;
        private readonly IVendorService vendorService;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;

        public CustomerOrderService(
            IRepository<CustomerOrder> customerOrderRepository,
            IProductService productService,
            IVendorOfferService vendorOfferService,
            ICustomerDeliveryAddressService customerDeliveryAddressService,
            ICustomerService customerService,
            IVendorService vendorService,
            IConfiguration configuration,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.customerOrderRepository = customerOrderRepository;
            this.productService = productService;
            this.vendorOfferService = vendorOfferService;
            this.customerDeliveryAddressService = customerDeliveryAddressService;
            this.customerService = customerService;
            this.vendorService = vendorService;
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IEnumerable<Common.DomainModels.CustomerOrder>> GetCustomerOrdersAsync(int customerId)
        {
            try
            {
                var customerOrders = await this.customerOrderRepository.GetAsync(filter: d => d.CustomerId == customerId).ConfigureAwait(false);
                return this.mapper.Map<IEnumerable<Common.DomainModels.CustomerOrder>>(customerOrders);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Common.DomainModels.CustomerOrder> GetCustomerOrderDetailsAsync(int customerOrderId)
        {
            try
            {
                var customerOrder = await this.customerOrderRepository.GetAsync(customerOrderId).ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.CustomerOrder>(customerOrder);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<(Common.DomainModels.CustomerOrder, string)> InsertCustomerOrderAsync(Common.DomainModels.CustomerOrder order)
        {
            try
            {
                var validationResult = await ValidateCustomerOrderAsync(order).ConfigureAwait(false);
                if (!validationResult.Equals("OK"))
                    return (null, validationResult);

                order.OrderedOn = DateTime.Now;
                order.Status = DataModel.Enums.OrderStatus.RECEIVED;
                order.VendorOfferId = order.VendorOfferId == 0 ? null : order.VendorOfferId;

                var orderToInsert = this.mapper.Map<CustomerOrder>(order);
                
                // Detach relation before saving, to avoid un-necessary operation and error(s)
                foreach (var customerOrderProduct in orderToInsert.CustomerOrderProducts)
                    customerOrderProduct.Product = null;

                this.customerOrderRepository.Insert(orderToInsert);
                await this.customerOrderRepository.SaveChangesAsync().ConfigureAwait(false);

                // Assign OrderNumber
                orderToInsert.OrderNumber = orderToInsert.CustomerId.ToString().PadLeft(7, '0');
                this.customerOrderRepository.Update(orderToInsert);
                await this.customerOrderRepository.SaveChangesAsync().ConfigureAwait(false);


                return (this.mapper.Map<Common.DomainModels.CustomerOrder>(orderToInsert), "OK");
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return (null, ex.Message);
            }
        }

        public async Task<(Common.DomainModels.CustomerOrder, string)> UpdateCustomerOrderAsync(Common.DomainModels.CustomerOrder order)
        {
            try
            {
                // Checks for Editing Customer Order
                var existingOrder = await this.customerOrderRepository.GetAsync(order.CustomerOrderId).ConfigureAwait(false);
                
                // Check if customer order exists
                if (existingOrder == null)
                    return (null, "CUSTOMER_ORDER_NOT_FOUND");

                // Check if customer order exists
                if (existingOrder.Status == DataModel.Enums.OrderStatus.CANCELLED)
                    return (null, "CANCELLED_ORDERED_CANNOT_BE_EDITED");

                // Check if customer order belongs to same customer
                if (existingOrder.CustomerId != order.CustomerId)
                    return (null, "WRONG_CUSTOMER_ID_USED");

                // Check if customer order belongs to same vendor
                if (existingOrder.VendorId != order.VendorId)
                    return (null, "WRONG_VENDOR_ID_USED");

                // Check order edit time limit
                var allowedTimeInMinutes = this.configuration.GetValue<int>("OrderUpdateTimeLimit");
                if (DateTime.Now.Subtract(existingOrder.EditedOn ?? DateTime.Now).TotalMinutes > allowedTimeInMinutes)
                    return (null, "CUSTOMER_ORDER_EDIT_TIMEOUT");

                existingOrder.EditedOn = DateTime.Now;
                existingOrder.CustomerOrderId = order.CustomerOrderId;
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.CustomerDeliveryAddressId = order.CustomerDeliveryAddressId;
                existingOrder.VendorId = order.VendorId;
                existingOrder.VendorOfferId = order.VendorOfferId == 0 ? null : order.VendorOfferId;

                // Add/Update CustomerOrderProducts based on ProductId
                var co_products = order.CustomerOrderProducts;
                var existing_co_products = existingOrder.CustomerOrderProducts;
                foreach (var co_product in co_products)
                {
                    var existing_co_product = existing_co_products.Where(d => d.ProductId == co_product.ProductId).FirstOrDefault();
                    if(existing_co_product != null)
                        co_product.CustomerOrderProductId = existing_co_product.CustomerOrderProductId;
                }
                
                var mapped_co = this.mapper.Map<Common.DomainModels.CustomerOrder>(existingOrder);
                mapped_co.CustomerOrderProducts = co_products;

                var validationResult = await ValidateCustomerOrderAsync(mapped_co).ConfigureAwait(false);
                if (!validationResult.Equals("OK"))
                    return (null, validationResult);

                existingOrder.CustomerOrderProducts = new List<CustomerOrderProduct>();
                existingOrder.CustomerOrderProducts = this.mapper.Map<ICollection<CustomerOrderProduct>>(co_products);
                
                // Detach relation before saving, to avoid un-necessary operation and error(s)
                foreach (var customerOrderProduct in existingOrder.CustomerOrderProducts)
                    customerOrderProduct.Product = null;

                await this.customerOrderRepository.SaveChangesAsync().ConfigureAwait(false);
                return (this.mapper.Map<Common.DomainModels.CustomerOrder>(existingOrder), "OK");
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return (null, ex.Message);
            }
        }

        public async Task<string> CancelCustomerOrderAsync(int customerOrderId)
        {
            try
            {
                var customerOrder = await this.customerOrderRepository.GetAsync(customerOrderId);
                if(customerOrder != null)
                {
                    // Check order edit time limit
                    var allowedTimeInMinutes = this.configuration.GetValue<int>("OrderUpdateTimeLimit");
                    if (DateTime.Now.Subtract(customerOrder.EditedOn ?? DateTime.Now).TotalMinutes > allowedTimeInMinutes)
                        return "CUSTOMER_ORDER_EDIT_TIMEOUT";

                    customerOrder.Status = DataModel.Enums.OrderStatus.CANCELLED;
                    await this.customerOrderRepository.SaveChangesAsync().ConfigureAwait(false);
                    return "OK";
                }

                return "CUSTOMER_ORDER_NOT_FOUND";
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> ValidateCustomerOrderAsync(Common.DomainModels.CustomerOrder customerOrder)
        {
            // Check if customer exists
            if (!await this.customerService.CheckCustomerExistsAsync(customerOrder.CustomerId).ConfigureAwait(false))
                return "CUSTOMER_NOT_FOUND";

            // Check CustomerDeliveryAddress
            var customerDeliveryAddress = await this.customerDeliveryAddressService.GetDeliveryAddressById(customerOrder.CustomerDeliveryAddressId).ConfigureAwait(false);
            
            // Check if CustomerDeliveryAddress exists
            if(customerDeliveryAddress == null)
                return "CUSTOMER_DELIVERY_ADDRESS_NOT_FOUND";

            // Check if CustomerDeliveryAddress belongs to same customer
            if (customerDeliveryAddress.CustomerId != customerOrder.CustomerId)
                return "WRONG_CUSTOMER_DELIVERY_ADDRESS_USED";

            // Check if vendor exists
            if (!await this.vendorService.CheckVendorExistsAsync(customerOrder.VendorId).ConfigureAwait(false))
                return "VENDOR_NOT_FOUND";

            // Check VendorOffer
            var usedVendorOfferId = Convert.ToInt32(customerOrder.VendorOfferId);
            if (usedVendorOfferId != 0)
            {
                var vendorOffer = await this.vendorOfferService.GetVendorOfferAsync(usedVendorOfferId).ConfigureAwait(false);

                // Check if offer exists
                if (vendorOffer == null)
                    return "VENDOR_OFFER_NOT_FOUND";

                // Check if offer belongs to same vendor
                if (vendorOffer.VendorId != customerOrder.VendorId)
                    return "WRONG_VENDOR_OFFER_USED";

                // Check if offer is valid
                if (!vendorOffer.IsActive)
                    return "VENDOR_OFFER_NOT_ACTIVE";

                // Check if offer is valid
                if (vendorOffer.ValidTill < DateTime.Now)
                    return "VENDOR_OFFER_EXPIRED";
            }

            // Check if product belongs to correct vendor
            foreach (var customerOrderProduct in customerOrder.CustomerOrderProducts)
            {
                if (customerOrderProduct.Quantity < 1)
                    return "PRODUCT_QUANTITY_CANNOT_BE_ZERO";

                var product = await this.productService.GetProductAsync(customerOrderProduct.ProductId).ConfigureAwait(false);

                // Check if product exists
                if (product == null)
                    return "PRODUCT_NOT_FOUND";

                if (product.VendorId == customerOrder.VendorId)
                    customerOrderProduct.Product = product;
                else
                    return "WRONG_PRODUCT_ADDED";
            }

            // Check if Order meets minimum cost criteria
            var allowedMinimumOrderValue = this.configuration.GetValue<int>("OrderMinimumValue");
            if (customerOrder.CustomerOrderProducts.Sum(d => d.Product.Price * d.Quantity) < allowedMinimumOrderValue)
                return $"CUSTOMER_ORDER_VALUE_LESS_THAN_{allowedMinimumOrderValue}";

            return "OK";
        }
    }
}
