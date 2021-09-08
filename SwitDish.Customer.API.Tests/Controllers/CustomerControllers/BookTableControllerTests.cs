using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    class BookTableControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase]
        public async Task BookCustomerTableTest_Returns_Ok()
        {
            // Arrage
            var data = new Dictionary<string, string>
            {
                { "FullName", "John Doe" },
                { "Email", "john@doe.com" },
                { "Phone", "1234567" },
                { "DateAndTime", "2020-01-01" },
                { "CustomerId", "1" },
                { "VendorId", "1" },
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.BOOK_CUSTOMER_TABLE, new FormUrlEncodedContent(data)).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customerBooking = JsonConvert.DeserializeObject<CustomerBookingViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("1234567", customerBooking.Phone);
            Assert.IsNotNull(customerBooking);
        }

        [TestCase]
        public async Task BookCustomerTableTest_Returns_BadRequest()
        {
            // Arrage
            var data = new Dictionary<string, string>
            {
                { "FullName", "John Doe" },
                { "Email", "john@doe.com" },
                { "Phone", "1234567" },
                { "CustomerId", "abc" },
                { "VendorId", "xyz" },
            };

            // Act
            var response = await TestClient.PostAsync(API_REQUESTS.BOOK_CUSTOMER_TABLE, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
