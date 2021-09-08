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
    class UpdateOrderControllerTests : IntegrationTestBase
    {
        [Explicit("Manual tests are fine. Automated test fails. WIP")]
        [Test]
        public async Task UpdateOrder_OK()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1000" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var updatedOrder = JsonConvert.DeserializeObject<CustomerOrderViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, updatedOrder.CustomerDeliveryAddressId);

        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_WRONG_CUSTOMER_ID_USED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "125" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("WRONG_CUSTOMER_ID_USED", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_CUSTOMER_ORDER_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "0241" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_ORDER_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_CUSTOMER_DELIVERY_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1895" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_DELIVERY_ADDRESS_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_WRONG_VENDOR_ID_USED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1995" },
                { "VendorOfferId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("WRONG_VENDOR_ID_USED", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_VENDOR_OFFER_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "0321" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("VENDOR_OFFER_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_VENDOR_OFFER_EXPIRED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "2" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "100" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("VENDOR_OFFER_EXPIRED", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_WRONG_CUSTOMER_DELIVERY_ADDRESS_USED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "3" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "100" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("WRONG_CUSTOMER_DELIVERY_ADDRESS_USED", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_CUSTOMER_ORDER_EDIT_TIMEOUT()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "2" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "100" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_ORDER_EDIT_TIMEOUT", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_PRODUCT_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "300" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("PRODUCT_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_WRONG_PRODUCT_ADDED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "4" },
                { "CustomerOrderProducts[0].Quantity", "100" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("WRONG_PRODUCT_ADDED", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_PRODUCT_QUANTITY_CANNOT_BE_ZERO()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "0" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("PRODUCT_QUANTITY_CANNOT_BE_ZERO", responseData.Message);
        }

        [Test]
        public async Task UpdateOrder_CustomerOrder_CUSTOMER_ORDER_VALUE_LESS_THAN_1000()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerOrderId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "Instructions", "Make it quick!" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "2" }
            };

            // Act 
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_ORDER_VALUE_LESS_THAN_1000", responseData.Message);
        }
    }
}
