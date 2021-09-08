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
    public class CustomerOrderFeedbackServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer;
        private DataModel.Models.Vendor Test_Vendor;
        private DataModel.Models.CustomerOrder Test_CustomerOrder;
        private DataModel.Models.CustomerDeliveryAddress Test_CustomerDeliveryaddress;
        private DataModel.Models.ProductCategory Test_ProductCategory;
        private DataModel.Models.Product Test_Product;
        private DataModel.Models.VendorOffer Test_VendorOffer;
        private CustomerOrderFeedbackService customerOrderFeedbackService;

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

            var newCustomerDeliveryaddress = new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = this.Test_Customer.CustomerId,
                CompleteAddress = "xyz street",
                DeliveryArea = "xyz town",
                Instructions = "xyz instructions",
                CustomerDeliveryAddressType = DataModel.Enums.CustomerDeliveryAddressType.Home
            };
            this.dbContext.CustomerDeliveryAddresses.Add(newCustomerDeliveryaddress);
            this.Test_CustomerDeliveryaddress = newCustomerDeliveryaddress;

            var newVendorOffer = new DataModel.Models.VendorOffer
            {
                VendorId = this.Test_Vendor.VendorId,
                OfferCode = "123a",
                Description = "test description",
                Title = "free coupon",
                ValidFrom = DateTime.Now,
                ValidTill = DateTime.Today,
                DiscountPercentage = 2,
                Image = "image.png",
                IsActive = true
            };
            this.dbContext.VendorOffers.Add(newVendorOffer);
            this.Test_VendorOffer = newVendorOffer;
            this.dbContext.SaveChanges();
            var newProductCategory = new DataModel.Models.ProductCategory
            {
                Name = "Essential",
            };
            this.dbContext.ProductCategories.Add(newProductCategory);
            this.Test_ProductCategory = newProductCategory;
            this.dbContext.SaveChanges();

            var newProduct = new DataModel.Models.Product
            {
                Name = "Test Product",
                Description = "Test desciption",
                Price = 100,
                Image = "product.png",
                ProductCategoryId = this.Test_ProductCategory.ProductCategoryId,
                VendorId = this.Test_Vendor.VendorId
            };
            this.dbContext.Products.Add(newProduct);
            this.Test_Product = newProduct;
            this.dbContext.SaveChanges();

            var testCustomerorder = new DataModel.Models.CustomerOrder
            {
                 CustomerId = this.Test_Customer.CustomerId,
                 OrderNumber ="1",
                 DeliveredOn = DateTime.Today,
                 DeliveryCharges = 100,
                 Tax = 2,
                 VendorId = this.Test_Vendor.VendorId,
                 CustomerDeliveryAddressId = this.Test_CustomerDeliveryaddress.CustomerDeliveryAddressId,
                 OrderedOn = DateTime.Now,
                 RestaurantCharges = 20,
                 Status = DataModel.Enums.OrderStatus.READY,
                 VendorOfferId = this.Test_Vendor.VendorId
            };
            this.dbContext.CustomerOrders.Add(testCustomerorder);
            this.Test_CustomerOrder = testCustomerorder;
            this.dbContext.SaveChanges();
            var customerOrderFeedbackRepository = new CustomerOrderFeedbackRepository(this.dbContext);
            this.customerOrderFeedbackService = new CustomerOrderFeedbackService(customerOrderFeedbackRepository, mapper, logger);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test()]
        public async Task InsertCustomerOrderFeedbackAsyncTest()
        {   
            // Arrange
            var testInsertCustomerOrderFeedback = new CustomerOrderFeedback()
            {
                CustomerId = this.Test_Customer.CustomerId,
                DeliveryAgentComments = "Test comments",
                AppComments = "Test app comments",
                FoodComments = "Test food comments",
                AppRating = 1,
                FoodRating=1,
                DeliveryAgentRating =1,
                CustomerOrderId = this.Test_CustomerOrder.CustomerOrderId,
                
            };

            // Act
            var result = await this.customerOrderFeedbackService.InsertCustomerOrderFeedbackAsync(testInsertCustomerOrderFeedback).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testInsertCustomerOrderFeedback.FoodComments,result.FoodComments );
            Assert.AreEqual(testInsertCustomerOrderFeedback.DeliveryAgentComments, result.DeliveryAgentComments);
            Assert.AreEqual(testInsertCustomerOrderFeedback.AppComments, result.AppComments);
        }
    }
}