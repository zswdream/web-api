using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.Repositories;
using SwitDish.Common.DomainModels;
using System.Threading.Tasks;
using System.Linq;
using System;
using SwitDish.Common.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class CustomerDeliveryAddressServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer;
        private DataModel.Models.Vendor Test_Vendor;
        private CustomerDeliveryAddressService customerDeliveryAddressService;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new DataModel.Models.SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();


            var newTestCustomer = new DataModel.Models.Customer
            {
                ApplicationUser = new DataModel.Models.ApplicationUser
                {
                    Email = "customer@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    FirstName = "Customer",
                    LastName = "User",
                    Address = new DataModel.Models.Address
                    {
                        BuildingNumber = "123",
                        AddressLine1 = "abc street",
                        Country = "SOMECOUNTRY"
                    }
                }
            };
            this.dbContext.Customers.Add(newTestCustomer);
            this.Test_Customer = newTestCustomer;

            var newTestVendor = new DataModel.Models.Vendor
            {
                Name = "ABC Vendor",
                BrandName = "ABC Brand",
                PrimaryEmail = "Email.gmail.com",
                VendorDeliveryTime = new DataModel.Models.VendorDeliveryTime
                {
                    Minimum = new TimeSpan(0, 20, 0),
                    Maximum = new TimeSpan(0, 30, 0)
                },
                Address = new DataModel.Models.Address
                {
                    AddressLine1 = "abc"
                }
            };
            this.dbContext.Vendors.Add(newTestVendor);
            this.Test_Vendor = newTestVendor;

            this.dbContext.SaveChanges();
            var customerDeliveryAddressRepository = new CustomerDeliveryAddressRepository(this.dbContext);
            var customerRepository = new CustomerRepository(this.dbContext);
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            
            var azureBlobClient = CloudStorageAccount.Parse(configuration.GetValue<string>("switdishcustomerimages-connectionstring")).CreateCloudBlobClient();
            var azureBlobService = new AzureBlobService(azureBlobClient);

            var customerService = new CustomerService(customerRepository, this.mapper, this.logger, azureBlobService, configuration);
            this.customerDeliveryAddressService = new CustomerDeliveryAddressService(customerDeliveryAddressRepository, mapper, logger, customerService);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task DeleteDeliveryAddressesAsyncTest()
        {
            // Arrange
            var testCustomerDeliveryAddress = new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "ABC",
                CompleteAddress = "XYZ"

            };
            this.dbContext.CustomerDeliveryAddresses.Add(testCustomerDeliveryAddress);
            this.dbContext.SaveChanges();

            // Act
            var result = await this.customerDeliveryAddressService.DeleteDeliveryAddressAsync(testCustomerDeliveryAddress.CustomerDeliveryAddressId).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetDeliveryAddressesAsyncTest_Success()
        {
            // Arrange
            this.dbContext.CustomerDeliveryAddresses.Add(new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "Test ABC"
                
            });
            this.dbContext.CustomerDeliveryAddresses.Add(new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "Test XYZ"
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await this.customerDeliveryAddressService.GetDeliveryAddressesAsync(this.Test_Customer.CustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2,result.Count());
        }

        [Test]
        public async Task GetDeliveryAddressesAsyncTest_Returns_0()
        {
            // Arrange
            var testCustomerId = 999;

            // Act
            var result = await this.customerDeliveryAddressService.GetDeliveryAddressesAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        [Test()]
        public async Task InsertDeliveryAddressesAsyncTest()
        {   
            // Arrange
            var testInsertDeliveryAddress = new CustomerDeliveryAddress()
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "ABC street "
            };

            // Act
            var result = await this.customerDeliveryAddressService.InsertDeliveryAddressAsync(testInsertDeliveryAddress).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, this.dbContext.CustomerDeliveryAddresses.Where(d=> d.CustomerId == this.Test_Customer.CustomerId).Count());
        }

        [Test]
        public async Task UpdateDeliveryAddressesAsyncTest()
        {
            // Arrange
            var newUpdateDeliveryAddress = new DataModel.Models.CustomerDeliveryAddress()
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "ABC street"  
            };
            this.dbContext.CustomerDeliveryAddresses.Add(newUpdateDeliveryAddress);
            this.dbContext.SaveChanges();
            this.dbContext.Entry(newUpdateDeliveryAddress).State = EntityState.Detached;
            var testUpdateDeliveryAddress = new CustomerDeliveryAddress
            {
                CustomerDeliveryAddressId = newUpdateDeliveryAddress.CustomerDeliveryAddressId,
                CustomerId = newUpdateDeliveryAddress.CustomerId,
                DeliveryArea = "XYZ Street"
            };

            // Act
            var result = await this.customerDeliveryAddressService.UpdateDeliveryAddressAsync(testUpdateDeliveryAddress).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUpdateDeliveryAddress.CustomerId, result.CustomerId);
            Assert.AreEqual(testUpdateDeliveryAddress.DeliveryArea, result.DeliveryArea);
        }
        [Test]
        public async Task UpdateDeliveryAddressesAsyncTest_False_Id()
        {
            // Arrange
            var testUpdateDeliveryAddress = new CustomerDeliveryAddress
            {
                CustomerId = 987445,
                DeliveryArea = "XYZ Street"
            };

            // Act
            var result = await this.customerDeliveryAddressService.UpdateDeliveryAddressAsync(testUpdateDeliveryAddress).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task InsertDeliveryAddressTest()
        {
            // Arrange
            var testCustomerDeliveryAddress = new CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "ABC Street"
            };

            // Act
            var result = await this.customerDeliveryAddressService.InsertDeliveryAddressAsync(testCustomerDeliveryAddress).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomerDeliveryAddress.DeliveryArea, result.DeliveryArea);
        }

        [Test]
        public async Task UpdateDeliveryAddressTest()
        {
            // Arrange
            var newCustomerDeliveryAddress = new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryArea = "ABC Street"
            };
            this.dbContext.CustomerDeliveryAddresses.Add(newCustomerDeliveryAddress);
            this.dbContext.SaveChanges();
            this.dbContext.Entry(newCustomerDeliveryAddress).State = EntityState.Detached;

            var testCustomerDeliveryAddress = new CustomerDeliveryAddress
            {
                CustomerDeliveryAddressId = newCustomerDeliveryAddress.CustomerDeliveryAddressId,
                CustomerId = newCustomerDeliveryAddress.CustomerId,
                DeliveryArea = "XYZ Street"
            };

            // Act
            var result = await this.customerDeliveryAddressService.UpdateDeliveryAddressAsync(testCustomerDeliveryAddress).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomerDeliveryAddress.CustomerId, result.CustomerId);
            Assert.AreEqual(testCustomerDeliveryAddress.DeliveryArea, result.DeliveryArea);
        }
        [Test]
        public async Task UpdateDeliveryAddressTest_Return_Null()
        {
            // Arrange
            var testCustomerDeliveryAddress = new CustomerDeliveryAddress
            { 
                CustomerId = 99999,
                DeliveryArea = "XYZ Street"
            };

            // Act
            var result = await this.customerDeliveryAddressService.UpdateDeliveryAddressAsync(testCustomerDeliveryAddress).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetDeliveryAddressByIdTest_Success()
        {
            // Arrange
            var testCustomerDeliveryAddress = new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = 1,
                CompleteAddress = "xyz"
            };

            this.dbContext.CustomerDeliveryAddresses.Add(testCustomerDeliveryAddress);
            this.dbContext.SaveChanges();
            // Act
            var result = await this.customerDeliveryAddressService.GetDeliveryAddressById(testCustomerDeliveryAddress.CustomerDeliveryAddressId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomerDeliveryAddress.CustomerDeliveryAddressId, result.CustomerDeliveryAddressId);
            Assert.AreEqual(testCustomerDeliveryAddress.CompleteAddress, result.CompleteAddress);
        }
        [Test]
        public async Task GetDeliveryAddressByIdTest_Returns_Null()
        {
            // Arrange
            var testCustomerId = 999;

            // Act
            var result = await this.customerDeliveryAddressService.GetDeliveryAddressById(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
    }
}