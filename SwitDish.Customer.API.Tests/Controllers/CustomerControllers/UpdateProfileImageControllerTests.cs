using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using NUnit.Framework;
using SwitDish.Common.Services;
using SwitDish.Customer.API.Controllers.CustomerControllers;
using SwitDish.Customer.API.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.CustomerControllers.Tests
{
    [TestFixture]
    public class UpdateProfileImageControllerTests : API.Tests.IntegrationTestBase
    {
        private List<string> TestFiles = new List<string>();

        [Test]
        public async Task UpdateProfileImageTest_Success()
        {
            // Arrange
            var file = File.Open("dummy.jpg", FileMode.Create);
            HttpContent fileStreamContent = new StreamContent(file);
            var formData = new MultipartFormDataContent
            {
                { new StringContent("1"), "CustomerId" },
                { fileStreamContent, "ImageFile", "dummy.jpg" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_CUSTOMER_PROFILE_IMAGE, formData).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var testFileUri = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TestFiles.Add(testFileUri.Split('/').LastOrDefault());
        }

        [Test]
        public async Task UpdateProfileImageTest_BadRequest()
        {
            // Arrange
            var file = File.Open("dummy.bad", FileMode.Create);
            HttpContent fileStreamContent = new StreamContent(file);
            var formData = new MultipartFormDataContent
            {
                { new StringContent("1"), "CustomerId" },
                { fileStreamContent, "ImageFile", "dummy.bad" }
            };

            // Act
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_CUSTOMER_PROFILE_IMAGE, formData).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TearDown]
        public async Task TearDown()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var azureBlobClient = CloudStorageAccount.Parse(configuration.GetValue<string>("switdishcustomerimages-connectionstring")).CreateCloudBlobClient();
            var azureBlobService = new AzureBlobService(azureBlobClient);

            // Remove test files from azure storage
            foreach (var file in TestFiles)
            {
                await azureBlobService.DeleteFileBlobAsync(configuration.GetValue<string>("ProfileImagesAzureStorageContainerName"), file).ConfigureAwait(false);
            }
        }
    }
}