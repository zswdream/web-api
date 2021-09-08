using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Tests;
using SwitDish.Customer.API.Utilities;
using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture]
    class DeleteVendorFeedbackReactionControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task Delete_Vendor_Feedback_Reaction_Test_Returns_BadRequest()
        {
            // Arrange
            var query = "CustomerId=1&CustomerOrderFeedbackId=0";

            // Act
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_VENDOR_FEEDBACK_REACTION}?{query}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Delete_Vendor_Feedback_Reaction_Test_Returns_NotFound()
        {
            // Arrange
            var query = "CustomerId=1&CustomerOrderFeedbackId=222&ReactionType=Like";

            // Act
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_VENDOR_FEEDBACK_REACTION}?{query}").ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Delete_Vendor_Feedback_Reaction_Test_Returns_Ok()
        {
            // Arrange
            var query = "CustomerId=2&CustomerOrderFeedbackId=2&ReactionType=Like";

            // Act
            var response = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_VENDOR_FEEDBACK_REACTION}?{query}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
