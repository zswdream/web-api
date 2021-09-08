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
    class AddVendorFeedbackReactionControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task Add_Vendor_Feedback_Reaction_Test_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderFeedbackId", "1" },
                { "ReactionType", "Average" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_VENDOR_FEEDBACK_REACTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Add_Vendor_Feedback_Reaction_Test_Returns_NotFound()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderFeedbackId", "4" },
                { "ReactionType", "Like" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_VENDOR_FEEDBACK_REACTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Add_Vendor_Feedback_Reaction_Test_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderFeedbackId", "2" },
                { "ReactionType", "Like" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_VENDOR_FEEDBACK_REACTION, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var result = JsonConvert.DeserializeObject<VendorFeedbackReactionViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual("Like", result.ReactionType.ToString());
        }
    }
}
