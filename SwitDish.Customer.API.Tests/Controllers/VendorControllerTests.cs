using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Common.ViewModels;
using SwitDish.Customer.API.Controllers;
using SwitDish.Customer.API.Models;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.Tests
{
    [TestFixture]
    public class VendorControllerTests : API.Tests.IntegrationTestBase
    {
        [Test]
        public async Task GetAllVendorsTest()
        {
            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_ALL_VENDORS}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var vendorList = JsonConvert.DeserializeObject<IEnumerable<VendorInfoViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, vendorList.Count());
        }

        [Test]
        public async Task GetAllVendorsTest_Filtered()
        {
            // Arrange
            var filtersQuery = "Cuisines=Chinese";

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_ALL_VENDORS}?{filtersQuery}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var vendorList = JsonConvert.DeserializeObject<IEnumerable<VendorInfoViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, vendorList.Count());
        }

        [Test]
        public async Task GetVendorTest_OK()
        {
            // Arrange
            var testVendorId = 1;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR}?vendorId={testVendorId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var vendor = JsonConvert.DeserializeObject<VendorDetailsViewModel>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(vendor);
        }

        [Test]
        public async Task GetVendorTest_NotFound()
        {
            // Arrange
            var testVendorId = 999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR}?vendorId={testVendorId}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetVendorGalleryTest_OK()
        {
            // Arrange
            var testVendorId = 1;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_GALLERY}?vendorId={testVendorId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var vendorGalleryImages = JsonConvert.DeserializeObject<IEnumerable<VendorGalleryImageViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, vendorGalleryImages.Count());
        }

        [Test]
        public async Task GetVendorGalleryTest_NotFound()
        {
            // Arrange
            var testVendorId = 999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_GALLERY}?vendorId={testVendorId}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetVendorOffersTest_OK()
        {
            // Arrange
            var testVendorId = 1;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_OFFERS}?vendorId={testVendorId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var vendorOffers = JsonConvert.DeserializeObject<IEnumerable<VendorOfferViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, vendorOffers.Count());
        }

        [Test]
        public async Task GetVendorOffersTest_NotFound()
        {
            // Arrange
            var testVendorId = 999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_OFFERS}?vendorId={testVendorId}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetVendorFeedbacksTest_OK()
        {
            // Arrange
            var testVendorId = 1;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_FEEDBACKS}?vendorId={testVendorId}").ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var vendorFeedbacks = JsonConvert.DeserializeObject<IEnumerable<VendorFeedbackViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, vendorFeedbacks.Count());
        }

        [Test]
        public async Task GetVendorFeedbackTest_NotFound()
        {
            // Arrange
            var testVendorId = 999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_FEEDBACKS}?vendorId={testVendorId}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetVendorFeedbacks_NotFound()
        {
            // Arrange
            var testVendorId = 999;

            // Act
            var response = await TestClient.GetAsync($"{API_REQUESTS.GET_VENDOR_FEEDBACKS}?vendorId={testVendorId}").ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetVendorListFilters_OK()
        {
            // Act
            var response = await TestClient.GetAsync(API_REQUESTS.GET_VENDOR_FILTERS).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetProductCategories_OK()
        {
            // Act
            var response = await TestClient.GetAsync(API_REQUESTS.GET_PRODUCT_CATEGORIES).ConfigureAwait(false);
            var responseData = JsonConvert.DeserializeObject<ApiResponseViewModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var productCategories = JsonConvert.DeserializeObject<IEnumerable<ProductCategoryViewModel>>(responseData.Data.ToString());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, productCategories.Count());
        }
    }
}