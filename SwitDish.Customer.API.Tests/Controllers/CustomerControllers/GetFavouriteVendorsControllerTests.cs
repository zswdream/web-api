using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture()]
    class GetFavouriteVendorsControllerTests : API.Tests.IntegrationTestBase
    {
        [TestCase(arg: 1, ExpectedResult = 1)]
        public async Task<int> GetFavouriteVendorsTest(int CustomerId)
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_FAVOURITE_VENDORS}?CustomerId={CustomerId}").ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var customerFavouriteVendors = JsonConvert.DeserializeObject<IEnumerable<Models.VendorDetailsViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return customerFavouriteVendors.Count();
        }
    }
}
