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
    class AddDeliveryAddressControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task AddCustomerDeliveryAddressTest_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "Instructions", "Please deliver quick" },
                { "CompleteAddress", "My House, This Street, That City" },
                { "DeliveryArea", "SomeArea" },
                { "Type", "Work" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_DELIVERY_ADDRESS, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customerDeliveryAddress = JsonConvert.DeserializeObject<CustomerDeliveryAddressViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("Please deliver quick", customerDeliveryAddress.Instructions);
            Assert.IsNotNull(customerDeliveryAddress);
        }

        [Test]
        public async Task AddCustomerDeliveryAddressTest_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerDeliveryAddressId", "abc" },
                { "Instructions", "Please deliver quick" },
                { "CompleteAddress", "My House, This Street, That City" },
                { "DeliveryArea", "SomeArea" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_DELIVERY_ADDRESS, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task AddCustomerDeliveryAddressTest_Full_Circle_Returns_Ok()
        {
            // Arrange
            var testCustomerId = 1;
            var data = new Dictionary<string, string>
            {
                { "CustomerId", testCustomerId.ToString() },
                { "Instructions", "Please deliver quick" },
                { "CompleteAddress", "My House, This Street, That City" },
                { "DeliveryArea", "SomeArea" },
                { "Type", "Work" }
            };

            // Act - Get All Delivery Addresses
            var response_initial = await TestClient.GetAsync($"{API_REQUESTS.GET_DELIVERY_ADDRESSES}?customerId={testCustomerId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_initial.Content.ReadAsStringAsync().ConfigureAwait(false));
            var initialCustomerDeliveryAddresses = JsonConvert.DeserializeObject<IEnumerable<CustomerDeliveryAddressViewModel>>(responseData.Data.ToString());

            // Assert - Get Delivery Addresses Success
            Assert.AreEqual(HttpStatusCode.OK, response_initial.StatusCode);

            // Act - Add new Delivery Address
            var response_add = await TestClient.PostAsync(API_REQUESTS.ADD_DELIVERY_ADDRESS, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_add.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customerDeliveryAddress = JsonConvert.DeserializeObject<CustomerDeliveryAddressViewModel>(responseData.Data.ToString());

            // Assert - Successfully add new Delivery Address
            Assert.AreEqual(HttpStatusCode.Created, response_add.StatusCode);
            Assert.AreEqual("Please deliver quick", customerDeliveryAddress.Instructions);
            Assert.IsNotNull(customerDeliveryAddress);

            // Act - Delete newly added Delivery Address
            var response_delete = await TestClient.DeleteAsync($"{API_REQUESTS.DELETE_DELIVERY_ADDRESS}?customerDeliveryAddressId={customerDeliveryAddress.CustomerDeliveryAddressId}").ConfigureAwait(false);
            
            // Assert - Successfully complete Delete Request
            Assert.AreEqual(HttpStatusCode.OK, response_delete.StatusCode);

            // Act - Get Delivery Addresses again
            var response_final = await TestClient.GetAsync($"{API_REQUESTS.GET_DELIVERY_ADDRESSES}?customerId={testCustomerId}").ConfigureAwait(false);
            responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response_final.Content.ReadAsStringAsync().ConfigureAwait(false));
            var finalCustomerDeliveryAddresses = JsonConvert.DeserializeObject<IEnumerable<CustomerDeliveryAddressViewModel>>(responseData.Data.ToString());

            // Assert - Initial and End results should be same
            Assert.AreEqual(initialCustomerDeliveryAddresses.Count(), finalCustomerDeliveryAddresses.Count());
        }



    }
}
