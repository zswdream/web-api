using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SwitDish.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Tests
{
    [TestFixture]
    public class SecurityQuestionServiceTests : UnitTestBase
    {
        private DataModel.Models.SecurityQuestion Test_SecurityQuestion_Pet;
        private DataModel.Models.SecurityQuestion Test_SecurityQuestion_School;
        private SecurityQuestionService securityQuestionService;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new DataModel.Models.SwitDishDbContext(this.dbOptions);
            this.dbContext.Database.OpenConnection();
            this.dbContext.Database.EnsureCreated();

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

            var securityQuestionRepository = new SecurityQuestionRepository(dbContext);
            this.securityQuestionService = new SecurityQuestionService(securityQuestionRepository, mapper, logger);
            this.DetachAllEntities();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetSecurityQuestionTest_Found()
        {
            // Arrange
            var testSecurityQuestionId = 1;

            // Act
            var result = await this.securityQuestionService.GetSecurityQuestion(testSecurityQuestionId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetSecurityQuestionTest_NotFound()
        {
            // Arrange
            var testSecurityQuestionId = 999;

            // Act
            var result = await this.securityQuestionService.GetSecurityQuestion(testSecurityQuestionId).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetSecurityQuestionsTest()
        {            
            // Act
            var result = await this.securityQuestionService.GetSecurityQuestions().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task InsertSecurityQuestionTest()
        {
            // Arrange
            var newQuestion = "How are you doing?";

            // Act
            var result = await this.securityQuestionService.InsertSecurityQuestion(newQuestion).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newQuestion, result.Item1.Question);
        }
    }
}