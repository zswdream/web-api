using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Repositories;
using SwitDish.Common.Services;
using SwitDish.Customer.Services;
using SwitDish.Customer.Services.Tests;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class CustomerServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer_John;
        private DataModel.Models.Customer Test_Customer_Doe;
        private CustomerService customerService;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new DataModel.Models.SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();

            
            var newTestCustomer_John = new DataModel.Models.Customer
            {
                ApplicationUser = new DataModel.Models.ApplicationUser
                {
                    Email = "john@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    FirstName = "John",
                    LastName = "Customer",
                    Phone = "12345678900",
                    DateOfBirth = DateTime.Now,
                    Address = new DataModel.Models.Address
                    {
                        BuildingNumber = "123",
                        AddressLine1 = "abc street",
                        Country = "SOMECOUNTRY"
                    }
                }
            };
            this.dbContext.Customers.Add(newTestCustomer_John);
            var newTestCustomer_Doe = new DataModel.Models.Customer
            {
                ApplicationUser = new DataModel.Models.ApplicationUser
                {
                    Email = "doe@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    FirstName = "Doe",
                    LastName = "Another Customer",
                    Phone = "12345678911",
                    DateOfBirth = DateTime.Now,
                    Address = new DataModel.Models.Address
                    {
                        BuildingNumber = "321",
                        AddressLine1 = "xyz street",
                        Country = "ANOTHER COUNTRY"
                    }
                }
            };
            this.dbContext.Customers.Add(newTestCustomer_Doe);
            this.Test_Customer_John = newTestCustomer_John;
            this.Test_Customer_Doe = newTestCustomer_Doe;
            this.dbContext.SaveChanges();

            var customerRepository = new CustomerRepository(dbContext);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            
            var azureBlobClient = CloudStorageAccount.Parse(configuration.GetValue<string>("switdishcustomerimages-connectionstring")).CreateCloudBlobClient();
            var azureBlobService = new AzureBlobService(azureBlobClient);

            this.customerService = new CustomerService(customerRepository, this.mapper, this.logger, azureBlobService, configuration);

            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetAllCustomersAsyncTest()
        {
            // Act
            var result = await this.customerService.GetAllCustomersAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(2 , result.Count());
        }
        [Test]
        public async Task GetCustomerByIdAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer_John;

            // Act
            var result = await this.customerService.GetCustomerByIdAsync(testCustomer.CustomerId).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(testCustomer.CustomerId, result.CustomerId);
            Assert.AreEqual(testCustomer.ApplicationUserId, result.ApplicationUserId);
        }
        [Test]
        public async Task GetCustomerByIdAsyncTest_Returns_Null()
        {
            // Arrange
            var testCustomerId = 989795;

            // Act
            var result = await this.customerService.GetCustomerByIdAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetCustomerByApplicationUserIdAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer_John;

            // Act
            var result = await this.customerService.GetCustomerByApplicationUserIdAsync(testCustomer.ApplicationUserId).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(testCustomer.CustomerId, result.CustomerId);
            Assert.AreEqual(testCustomer.ApplicationUserId, result.ApplicationUserId);
        }
        [Test]
        public async Task GetCustomerByApplicationUserIdAsyncTest_Returns_Null()
        {
            // Arrange
            var testApplicationUserId = 989795;

            // Act
            var result = await this.customerService.GetCustomerByApplicationUserIdAsync(testApplicationUserId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetCustomerByEmailAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer_Doe;

            // Act
            var result = await this.customerService.GetCustomerByEmailAsync(testCustomer.ApplicationUser.Email).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(testCustomer.CustomerId, result.CustomerId);
            Assert.AreEqual(testCustomer.ApplicationUserId, result.ApplicationUserId);
        }
        [Test]
        public async Task GetCustomerByEmailAsyncTest_Returns_Null()
        {
            // Arrange
            var testCustomerEmail = "bfd@gmail.com";

            // Act
            var result = await this.customerService.GetCustomerByEmailAsync(testCustomerEmail).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetCustomerByPhoneAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer_Doe;

            // Act
            var result = await this.customerService.GetCustomerByPhoneAsync(testCustomer.ApplicationUser.Phone).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(testCustomer.CustomerId, result.CustomerId);
            Assert.AreEqual(testCustomer.ApplicationUserId, result.ApplicationUserId);
        }
        [Test]
        public async Task GetCustomerByPhoneAsyncTest_Returns_Null()
        {
            // Arrange
            var testCustomerPhone = "112222";

            // Act
            var result = await this.customerService.GetCustomerByPhoneAsync(testCustomerPhone).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task InsertCustomerAsyncTest_Success()
        {
            // Arrange
            var testCustomer = new Common.DomainModels.Customer()
            {
                ApplicationUser = new Common.DomainModels.ApplicationUser
                {
                    FirstName = "Another",
                    LastName = "User",
                    Email = "new@email.com",
                    PasswordHash = "afdsgkjdhgks",
                    Phone = "123456",
                    Address = new Common.DomainModels.Address
                    {
                        AddressLine1 = "ABC Street"
                    },
                    Image = "image.png"
                }
            };

            // Act
            var result = await this.customerService.InsertCustomerAsync(testCustomer).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.ApplicationUserId);
            Assert.AreEqual(testCustomer.ApplicationUser.Email, result.ApplicationUser.Email);
        }
        [Test]
        public async Task UpdateCustomerAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer_John;
            this.dbContext.Entry(testCustomer).State = EntityState.Detached;
            testCustomer.ApplicationUser.Phone = "1122334455";
            this.dbContext.SaveChanges();
            // Act
            var result = await this.customerService.UpdateCustomerAsync(this.mapper.Map<Common.DomainModels.Customer>(testCustomer)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomer.ApplicationUser.FirstName, result.ApplicationUser.FirstName);
            Assert.AreEqual(testCustomer.ApplicationUser.LastName, result.ApplicationUser.LastName);
            Assert.AreEqual(testCustomer.ApplicationUser.Phone, result.ApplicationUser.Phone);
        }
        [Test]
        public async Task DeleteCustomerAsyncTest()
        {
            // Arrange
            var customerToDelete = this.Test_Customer_Doe;
            // Act
            var result = await this.customerService.DeleteCustomerAsync(this.mapper.Map<Common.DomainModels.Customer>(customerToDelete)).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task UpdateCustomerProfileImageAsync_Success()
        {
            // Arrange
            var testFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "file.jpg", "file.jpg");
            var testCustomerId = this.Test_Customer_John.CustomerId;

            // Act
            var result = await this.customerService.UpdateCustomerProfileImageAsync(testCustomerId, testFile).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual("ERROR", result);
        }
        [Test]
        public async Task UpdateCustomerProfileAsync()
        {
            // Arrange
            var testCustomer = this.Test_Customer_John;
            this.dbContext.Entry(testCustomer).State = EntityState.Detached; 
            testCustomer.ApplicationUser.FirstName = "DOE";
            testCustomer.ApplicationUser.LastName = "JOHN";
            testCustomer.ApplicationUser.Email = "TEST@email.com";
            testCustomer.ApplicationUser.Phone = "0610000000";
            this.dbContext.SaveChanges();
            // Act
            var result = await this.customerService.UpdateCustomerProfileAsync(this.mapper.Map<Common.DomainModels.Customer>(testCustomer)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomer.ApplicationUser.FirstName, result.ApplicationUser.FirstName);
            Assert.AreEqual(testCustomer.ApplicationUser.LastName, result.ApplicationUser.LastName);
            Assert.AreEqual(testCustomer.ApplicationUser.Email, result.ApplicationUser.Email);
            Assert.AreEqual(testCustomer.ApplicationUser.Phone, result.ApplicationUser.Phone);
        }
    }
}