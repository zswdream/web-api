using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<DataModel.Models.Customer> customerRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        private readonly IAzureBlobStorageService blobService;
        private readonly IConfiguration configuration;
        private readonly string ProfileImagesAzureStorageContainerName;
        public CustomerService(
            IRepository<DataModel.Models.Customer> customerRepository,
            IMapper mapper,
            ILoggerManager logger,
            IAzureBlobStorageService blobService,
            IConfiguration configuration)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.blobService = blobService;
            this.configuration = configuration;
            this.ProfileImagesAzureStorageContainerName = this.configuration.GetValue<string>("ProfileImagesAzureStorageContainerName");
        }

        public async Task<bool> CheckCustomerExistsAsync(int Id)
        {
            try
            {
                var query = await this.customerRepository.GetAsync(filter: d=> d.CustomerId == Id, tracking: false).ConfigureAwait(false);
                return query.Count() > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }
        public async Task<IEnumerable<Common.DomainModels.Customer>> GetAllCustomersAsync()
        {
            try
            {
                var customerDtos = await customerRepository.GetAllAsync().ConfigureAwait(false);
                var customers = this.mapper.Map<IEnumerable<Common.DomainModels.Customer>>(customerDtos);
                return customers;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<Common.DomainModels.Customer> GetCustomerByIdAsync(int Id)
        {
            try
            {
                var customer = await this.customerRepository.GetAsync(Id);
                if(customer != null)
                    return this.mapper.Map<Common.DomainModels.Customer>(customer);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<Common.DomainModels.Customer> GetCustomerByApplicationUserIdAsync(int Id)
        {
            try
            {
                var query = await this.customerRepository.GetAsync(filter: d=> d.ApplicationUserId == Id);
                var customer = query.FirstOrDefault();

                if (customer != null)
                    return this.mapper.Map<Common.DomainModels.Customer>(customer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<Common.DomainModels.Customer> GetCustomerByEmailAsync(string email)
        {
            try
            {
                var query = await customerRepository.GetAsync(filter: d=> d.ApplicationUser.Email == email).ConfigureAwait(false);
                var customer = query.FirstOrDefault();
                if(customer != null)
                    return this.mapper.Map<Common.DomainModels.Customer>(customer);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<Common.DomainModels.Customer> GetCustomerByPhoneAsync(string phone)
        {
            try
            {
                var query = await customerRepository.GetAsync(filter: d => d.ApplicationUser.Phone == phone).ConfigureAwait(false);
                var customer = query.FirstOrDefault();
                if (customer != null)
                    return this.mapper.Map<Common.DomainModels.Customer>(customer);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return null;
        }
        public async Task<Common.DomainModels.Customer> InsertCustomerAsync(Common.DomainModels.Customer customerDto)
        {
            try
            {
                var customer = this.mapper.Map<DataModel.Models.Customer>(customerDto);

                customer.ApplicationUser.Email = customer.ApplicationUser.Email.ToLower();
                
                // Hash password before saving
                customer.ApplicationUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(customer.ApplicationUser.PasswordHash);
                
                customerRepository.Insert(customer);
                await customerRepository.SaveChangesAsync().ConfigureAwait(false);

                return this.mapper.Map<Common.DomainModels.Customer>(customer);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<Common.DomainModels.Customer> UpdateCustomerAsync(Common.DomainModels.Customer customerDto)
        {
            try
            {
                var customer = this.mapper.Map<DataModel.Models.Customer>(customerDto);
                customer.ApplicationUser.Email = customer.ApplicationUser.Email.ToLower();
                customerRepository.Update(customer);
                await customerRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.Customer>(customer);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<bool> DeleteCustomerAsync(Common.DomainModels.Customer customerDto)
        {
            try
            {
                var customer = this.mapper.Map<DataModel.Models.Customer>(customerDto);
                customerRepository.Delete(customer);
                await customerRepository.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return false;
            }
        }
        public async Task<Common.DomainModels.Customer> UpdateCustomerProfileAsync(Common.DomainModels.Customer customer)
        {
            try
            {
                var existingCustomer = await this.customerRepository.GetAsync(customer.CustomerId).ConfigureAwait(false);
                existingCustomer.ApplicationUser.FirstName = customer.ApplicationUser.FirstName;
                existingCustomer.ApplicationUser.LastName = customer.ApplicationUser.LastName;
                existingCustomer.ApplicationUser.Email = customer.ApplicationUser.Email;
                existingCustomer.ApplicationUser.Phone = customer.ApplicationUser.Phone;
                existingCustomer.ApplicationUser.DateOfBirth = customer.ApplicationUser.DateOfBirth;

                this.customerRepository.Update(existingCustomer);
                await this.customerRepository.SaveChangesAsync().ConfigureAwait(false);
                return this.mapper.Map<Common.DomainModels.Customer>(existingCustomer);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<string> UpdateCustomerProfileImageAsync(int customerId, IFormFile imageFile)
        {
            try
            {
                // Check if customer exists
                var customer = await this.customerRepository.GetAsync(customerId).ConfigureAwait(false);
                if (customer == null) return "CUSTOMER_NOT_FOUND";

                // Delete old image if exists
                if (!string.IsNullOrEmpty(customer.ApplicationUser.Image))
                {
                    var profileImageFileName = customer.ApplicationUser.Image.Split('/').LastOrDefault();
                    await this.blobService.DeleteFileBlobAsync(this.ProfileImagesAzureStorageContainerName, profileImageFileName).ConfigureAwait(false);
                }

                // Assign random name to file
                var newGuid = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(imageFile.FileName);
                var fileName = newGuid+extension;

                var image_uri = await this.blobService.UploadFileBlobAsync(this.ProfileImagesAzureStorageContainerName, fileName, imageFile).ConfigureAwait(false);

                // Update Image URI in database
                customer.ApplicationUser.Image = image_uri.AbsoluteUri;

                // Update Customer Image URI in database
                this.customerRepository.Update(customer);
                await this.customerRepository.SaveChangesAsync().ConfigureAwait(false);
                
                return image_uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return "ERROR";
            }
        }
    }
}
