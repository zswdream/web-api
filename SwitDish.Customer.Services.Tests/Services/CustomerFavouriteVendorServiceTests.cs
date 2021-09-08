using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.Repositories;
using SwitDish.Customer.Services.Tests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class CustomerFavouriteVendorServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer;
        private DataModel.Models.Vendor Test_Vendor;        
        private CustomerFavouriteVendorService customerFavouriteVendorService;
        
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

            var newCustomerFavouriteVendor = new DataModel.Models.CustomerFavouriteVendor
            {
                CustomerId = this.Test_Customer.CustomerId,
                VendorId = this.Test_Vendor.VendorId
            };
            this.dbContext.CustomerFavouriteVendors.Add(newCustomerFavouriteVendor);
            this.dbContext.SaveChanges();

            var customerFavouriteVendorRepository = new CustomerFavouriteVendorRepository(dbContext);
            this.customerFavouriteVendorService = new CustomerFavouriteVendorService(customerFavouriteVendorRepository, mapper, logger);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetCustomerFavouriteVendorsAsyncTest_Success()
        {
            // Arrange
            var testCustomer = this.Test_Customer;
            // Act
            var result = await this.customerFavouriteVendorService.GetCustomerFavouriteVendorsAsync(testCustomer.CustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.Count());
        }
        [Test]
        public async Task GetCustomerFavouriteVendorsAsyncTest_Returns_0()
        {
            // Arrange
            var testCustomerId = 98989;
          
            // Act
            var result = await this.customerFavouriteVendorService.GetCustomerFavouriteVendorsAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task AddVendorAsFavouriteAsyncTest()
        {
            // Arrange
            var testCustomerId = this.Test_Customer.CustomerId;
            var testVendorId = this.Test_Vendor.VendorId;

            // Act
            await this.customerFavouriteVendorService.RemoveVendorAsFavouriteAsync(testCustomerId, testVendorId).ConfigureAwait(false);
            var result = await this.customerFavouriteVendorService.AddVendorAsFavouriteAsync(testCustomerId, testVendorId).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveVendorAsFavouriteAsyncTest()
        {
            // Arrange
            var testCustomerId = this.Test_Customer.CustomerId;
            var testVendorId = this.Test_Vendor.VendorId;

            // Act
            var result = await this.customerFavouriteVendorService.RemoveVendorAsFavouriteAsync(testCustomerId, testVendorId).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
        }
    }
}