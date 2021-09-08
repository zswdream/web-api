using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Repositories;
using SwitDish.Customer.Services;
using SwitDish.Customer.Services.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture()]
    public class CustomerSecurityQuestionServiceTests : UnitTestBase
    {
        private DataModel.Models.Customer Test_Customer;
        private DataModel.Models.SecurityQuestion Test_SecurityQuestion_Pet;
        private DataModel.Models.SecurityQuestion Test_SecurityQuestion_School;
        private DataModel.Models.CustomerSecurityQuestion Test_CustomerSecurityQuestion_Pet;
        private DataModel.Models.CustomerSecurityQuestion Test_CustomerSecurityQuestion_School;
        private CustomerSecurityQuestionService customerSecurityQuestionService;
        
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
            this.dbContext.SaveChanges();
            this.Test_Customer = newTestCustomer;

            var newTestSecurityQuestion_Pet = new DataModel.Models.SecurityQuestion
            {
                Question = "What's your pet name?"
            };
            var newTestSecurityQuestion_School = new DataModel.Models.SecurityQuestion
            {
                Question = "What's your first school name?"
            };
            this.dbContext.SecurityQuestions.Add(newTestSecurityQuestion_Pet);
            this.dbContext.SecurityQuestions.Add(newTestSecurityQuestion_School);
            this.dbContext.SaveChanges();
            this.Test_SecurityQuestion_Pet = newTestSecurityQuestion_Pet;
            this.Test_SecurityQuestion_School = newTestSecurityQuestion_School;

            var testCustomerSecurityQuestion_Pet = new DataModel.Models.CustomerSecurityQuestion
            {
                CustomerId = this.Test_Customer.CustomerId,
                SecurityQuestionId = this.Test_SecurityQuestion_Pet.SecurityQuestionId,
                Answer = "Peter"
            };
            this.dbContext.CustomerSecurityQuestions.Add(testCustomerSecurityQuestion_Pet);
            this.dbContext.SaveChanges();
            this.Test_CustomerSecurityQuestion_Pet = testCustomerSecurityQuestion_Pet;

            var customerSecurityQuestionRepository = new CustomerSecurityQuestionRepository(dbContext);
            this.customerSecurityQuestionService = new CustomerSecurityQuestionService(customerSecurityQuestionRepository, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetCustomerSecurityQuestionsAsyncTest_Success()
        {
            // Arrange 
            var testCustomerId = this.Test_Customer.CustomerId;

            // Act
            var result = await this.customerSecurityQuestionService.GetCustomerSecurityQuestionsAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.Count());
        }
        [Test]
        public async Task GetCustomerSecurityQuestionsAsyncTest_Returns_0()
        {
            // Arrange 
            var testCustomerId = 00000;
            
            // Act
            var result = await this.customerSecurityQuestionService.GetCustomerSecurityQuestionsAsync(testCustomerId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        [Test]
        public async Task AddSecurityQuestionAsyncTest()
        {
            // Arrange
            var testCustomerSecurityQuestion = new CustomerSecurityQuestion
            {
                SecurityQuestionId = this.Test_SecurityQuestion_School.SecurityQuestionId,
                Answer = "ABC School",
                CustomerId = this.Test_Customer.CustomerId
            };

            // Act 
            var result = await this.customerSecurityQuestionService.AddSecurityQuestionAsync(this.mapper.Map<CustomerSecurityQuestion>(testCustomerSecurityQuestion)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomerSecurityQuestion.Answer, result.Answer);
        }
        [Test]
        public async Task UpdateSecurityQuestionAsyncTest()
        {
            // Arrange
            var testCustomerSecurityQuestion = this.Test_CustomerSecurityQuestion_Pet;
            testCustomerSecurityQuestion.Answer = "New Pet";

            // Act 
            var result = await this.customerSecurityQuestionService.UpdateSecurityQuestionAsync(this.mapper.Map<CustomerSecurityQuestion>(testCustomerSecurityQuestion)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testCustomerSecurityQuestion.Answer, result.Answer);
        }

    }
}