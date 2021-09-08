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
    class AddOrderFeedbackControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task PostOrderFeedback_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "FoodRating", "1" },
                { "AppRating", "2" },
                { "DeliveryAgentRating", "4" },
                { "AppComments", "its good!" },
                { "FoodComments", "Delicious" },
                { "DeliveryAgentComments", "Wow" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER_FEEDBACK, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var postOrderFeedback = JsonConvert.DeserializeObject<CustomerOrderFeedbackViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(1, postOrderFeedback.CustomerId);
            Assert.AreEqual("its good!", postOrderFeedback.AppComments);
            
        }

        [Test]
        public async Task PostOrderFeedback_MissingRating_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "abc" },
                { "FoodRating", "1" },
                { "AppRating", "2" },
                { "AppComments", "its good!" },
                { "FoodComments", "Delicious" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER_FEEDBACK, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task PostOrderFeedback_InvalidRatingValue_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "abc" },
                { "FoodRating", "1" },
                { "AppRating", "2" },
                { "DeliveryAgentRating", "8" },
                { "AppComments", "its good!" },
                { "FoodComments", "Delicious" },
                { "DeliveryAgentComments", "Delicious" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER_FEEDBACK, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
