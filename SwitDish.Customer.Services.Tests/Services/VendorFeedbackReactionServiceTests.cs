using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Repositories;
using SwitDish.Customer.Services;
using SwitDish.Customer.Services.Tests;
using SwitDish.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class VendorFeedbackReactionServiceTests : UnitTestBase
    {
        private DataModel.Models.VendorFeedbackReaction Test_Reaction;
        private VendorFeedbackReactionService vendorFeedbackReactionService;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();

            var testCustomer = new DataModel.Models.Customer
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
            this.dbContext.Customers.Add(testCustomer);

            var testVendor = new DataModel.Models.Vendor
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
            this.dbContext.Vendors.Add(testVendor);
            this.dbContext.SaveChanges();

            var testCustomerDeliveryaddress = new DataModel.Models.CustomerDeliveryAddress
            {
                CustomerId = testCustomer.CustomerId,
                CompleteAddress = "xyz street",
                DeliveryArea = "xyz town",
                Instructions = "xyz instructions",
                CustomerDeliveryAddressType = DataModel.Enums.CustomerDeliveryAddressType.Home
            };
            this.dbContext.CustomerDeliveryAddresses.Add(testCustomerDeliveryaddress);

            var testProductCategory = new DataModel.Models.ProductCategory
            {
                Name = "Essential",
            };
            this.dbContext.ProductCategories.Add(testProductCategory);
            this.dbContext.SaveChanges();

            var testProduct = new DataModel.Models.Product
            {
                Name = "Test Product",
                Description = "Test desciption",
                Price = 100,
                Image = "product.png",
                ProductCategoryId = testProductCategory.ProductCategoryId,
                VendorId = testVendor.VendorId
            };
            this.dbContext.Products.Add(testProduct);
            this.dbContext.SaveChanges();

            var testCustomerOrder = new DataModel.Models.CustomerOrder
            {
                CustomerId = testCustomer.CustomerId,
                OrderNumber = "1",
                DeliveredOn = DateTime.Today,
                DeliveryCharges = 100,
                Tax = 2,
                VendorId = testVendor.VendorId,
                CustomerDeliveryAddressId = testCustomerDeliveryaddress.CustomerDeliveryAddressId,
                OrderedOn = DateTime.Now,
                RestaurantCharges = 20,
                Status = DataModel.Enums.OrderStatus.READY
            };

            this.dbContext.CustomerOrders.Add(testCustomerOrder);
            this.dbContext.SaveChanges();

            var testCustomerOrderFeedback = new DataModel.Models.CustomerOrderFeedback
            {
                CustomerId = testCustomer.CustomerId,
                DeliveryAgentComments = "Test comments",
                AppComments = "Test app comments",
                FoodComments = "Test food comments",
                AppRating = 1,
                FoodRating = 1,
                DeliveryAgentRating = 1,
                CustomerOrderId = testCustomerOrder.CustomerOrderId
            };
            this.dbContext.CustomerOrderFeedbacks.Add(testCustomerOrderFeedback);
            this.dbContext.SaveChanges();

            var testReaction = new DataModel.Models.VendorFeedbackReaction
            {
                CustomerId = testCustomer.CustomerId,
                CustomerOrderFeedbackId = testCustomerOrderFeedback.CustomerOrderFeedbackId,
                ReactionType = DataModel.Enums.VendorFeedbackReactionType.Like
            };
            this.dbContext.VendorFeedbackReactions.Add(testReaction);
            this.Test_Reaction = testReaction;
            this.dbContext.SaveChanges();

            var vendorFeedbackReactionRepository = new VendorFeedbackReactionRepository(dbContext);
            this.vendorFeedbackReactionService = new VendorFeedbackReactionService(vendorFeedbackReactionRepository, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetVendorFeedbackReactionsAsyncTest()
        {
            // Arrange
            var testFeedbackId = 1;

            // Act
            var result = await this.vendorFeedbackReactionService.GetVendorFeedbackReactionsAsync(testFeedbackId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task DeleteVendorFeedbackReactionAsyncTest()
        {
            // Arrange
            var testFeedbackReaction = new Common.DomainModels.VendorFeedbackReaction
            {
                CustomerId = this.Test_Reaction.CustomerId,
                CustomerOrderFeedbackId = this.Test_Reaction.CustomerOrderFeedbackId,
                ReactionType = this.Test_Reaction.ReactionType
            };

            // Act
            var result = await this.vendorFeedbackReactionService.DeleteVendorFeedbackReactionAsync(testFeedbackReaction);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task InsertVendorFeedbackReactionAsyncTest()
        {
            // Arrange
            var testFeedbackReaction = new Common.DomainModels.VendorFeedbackReaction
            {
                CustomerId = this.Test_Reaction.CustomerId,
                CustomerOrderFeedbackId = this.Test_Reaction.CustomerOrderFeedbackId,
                ReactionType = DataModel.Enums.VendorFeedbackReactionType.Dislike
            };

            // Act
            var result = await this.vendorFeedbackReactionService.InsertVendorFeedbackReactionAsync(testFeedbackReaction);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(DataModel.Enums.VendorFeedbackReactionType.Dislike, result.ReactionType);
        }
    }
}