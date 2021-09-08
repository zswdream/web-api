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
    class AddOrderControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task PostOrderTest_Returns_Ok()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "Instructions", "xyz street" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "30" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var postOrder = JsonConvert.DeserializeObject<CustomerOrderViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(1, postOrder.CustomerId);
            
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "abc" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Test]
        public async Task PostOrderTest_Returns_BadRequest_CUSTOMER_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "999" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_CUSTOMER_DELIVERY_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "178" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_DELIVERY_ADDRESS_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_VENDOR_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1527" },
                { "VendorOfferId", "1" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("VENDOR_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_VENDOR_OFFER_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "021" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("VENDOR_OFFER_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_VENDOR_OFFER_EXPIRED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "VendorOfferId", "2" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1000" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("VENDOR_OFFER_EXPIRED", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_WRONG_CUSTOMER_DELIVERY_ADDRESS_USED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "3" },
                { "VendorId", "1" },
                { "VendorOfferId", "1" },
                { "CustomerOrderProducts[0].ProductId", "3" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("WRONG_CUSTOMER_DELIVERY_ADDRESS_USED", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_WRONG_PRODUCT_ADDED()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "CustomerOrderProducts[0].ProductId", "4" },
                { "CustomerOrderProducts[0].Quantity", "100" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("WRONG_PRODUCT_ADDED", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_PRODUCT_NOT_FOUND()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "CustomerOrderProducts[0].ProductId", "9" },
                { "CustomerOrderProducts[0].Quantity", "1" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("PRODUCT_NOT_FOUND", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_PRODUCT_QUANTITY_CANNOT_BE_ZERO()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "CustomerOrderProducts[0].ProductId", "1" },
                { "CustomerOrderProducts[0].Quantity", "0" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("PRODUCT_QUANTITY_CANNOT_BE_ZERO", responseData.Message);
        }

        [Test]
        public async Task PostOrderTest_Returns_BadRequest_CUSTOMER_ORDER_VALUE_LESS_THAN_1000()
        {
            // Arrange
            var data = new Dictionary<string, string>
            {
                { "CustomerId", "1" },
                { "CustomerDeliveryAddressId", "1" },
                { "VendorId", "1" },
                { "CustomerOrderProducts[0].ProductId", "2" },
                { "CustomerOrderProducts[0].Quantity", "5" }
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.ADD_ORDER, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("CUSTOMER_ORDER_VALUE_LESS_THAN_1000", responseData.Message);
        }

    }
}
