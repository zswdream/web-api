using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.Repositories;
using SwitDish.DataModel.Models;
using System;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class CustomerBookingServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer;
        private DataModel.Models.Vendor Test_Vendor;
        private CustomerBookingService customerBookingService;

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
                Address = new Address
                {
                    AddressLine1 = "abc"
                }
            };
            this.dbContext.Vendors.Add(newTestVendor);
            this.dbContext.SaveChanges();
            this.Test_Customer = newTestCustomer;
            this.Test_Vendor = newTestVendor;

            var customerBookingRepository = new CustomerBookingRepository(this.dbContext);
            this.customerBookingService = new CustomerBookingService(customerBookingRepository, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task BookCustomerTableAsyncTest_True()
        {
            // Arrange
            var customerBooking = new Common.DomainModels.CustomerBooking
            {
                CustomerId = this.Test_Customer.CustomerId,
                VendorId = this.Test_Vendor.VendorId,
                DateTime = DateTime.Now,
                Email = "test@email.com",
                FullName = "John Doe",
                Phone = "12345"
            };

            // Act
            var result = await this.customerBookingService.BookCustomerTableAsync(customerBooking).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task BookCustomerTableAsyncTest_False()
        {
            // Arrange
            var customerBooking = new Common.DomainModels.CustomerBooking
            {
                CustomerId = 1,
                Email = "test@email.com",
                FullName = "John Doe",
                Phone = "12345"
            };

            // Act
            var result = await this.customerBookingService.BookCustomerTableAsync(customerBooking).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }
    }
}