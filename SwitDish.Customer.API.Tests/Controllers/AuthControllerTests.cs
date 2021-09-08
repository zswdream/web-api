using NUnit.Framework;
using SwitDish.Customer.API.Tests;
using SwitDish.Customer.API.Utilities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Controllers.Tests
{
    [TestFixture]
    public class AuthControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task LoginTest_UnAuthorized()
        {
            // ACT
            var data = new Dictionary<string, string>
            {
                { "Email", "test@email.com" },
                { "Password", "12345" }
            };

            var response = await TestClient.PostAsync(API_REQUESTS.LOGIN, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public async Task LoginTest_Authorized()
        {
            var data = new Dictionary<string, string>
            {
                { "Email", "switdish.test@gmail.com" },
                { "Password", "password321" }
            };
            var response = await TestClient.PostAsync(API_REQUESTS.LOGIN, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Explicit("Sometimes google blocks gmail account so skip this on build till actual account is used")]
        public async Task RequestPasswordResetTest_OK()
        {
            var testEmail = "switdish.test@gmail.com";
            var response = await TestClient.GetAsync($"{API_REQUESTS.REQUEST_PASSWORD_RESET}?email={testEmail}").ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ResetPasswordTest_DifferentPasswords()
        {
            var data = new Dictionary<string, string>
            {
                { "Email", "test@email.com" },
                { "VerificationCode", "12345" },
                { "NewPassword", "password" },
                { "ConfirmPassword", "differentpassword" }
            };
            var response = await TestClient.PostAsync(API_REQUESTS.RESET_PASSWORD, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task ResetPasswordTest_BadRequest()
        {
            var data = new Dictionary<string, string>
            {
                { "Email", "switdish.test@gmail.com" },
                { "VerificationCode", "527107" },
                { "NewPassword", "newwpassword" },
                { "ConfirmPassword", "newwpassword" }
            };
            var response = await TestClient.PostAsync(API_REQUESTS.RESET_PASSWORD, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task UpdatePasswordTest_Success()
        {
            var data = new Dictionary<string, string>
            {
                { "ApplicationUserId", "1" },
                { "OldPassword", "password123" },
                { "NewPassword", "password1" },
                { "ConfirmPassword", "password1" }
            };
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PASSWORD, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        [Test]
        public async Task UpdatePasswordTest_DifferentPasswords()
        {
            var data = new Dictionary<string, string>
            {
                { "ApplicationUserId", "1" },
                { "OldPassword", "password" },
                { "NewPassword", "password1" },
                { "ConfirmPassword", "password2" }
            };
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PASSWORD, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Test]
        public async Task UpdatePasswordTest_User_Not_Found()
        {
            var data = new Dictionary<string, string>
            {
                { "ApplicationUserId", "xxyyzz" },
                { "OldPassword", "password" },
                { "NewPassword", "password1" },
                { "ConfirmPassword", "password1" }
            };
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PASSWORD, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Test]
        public async Task UpdatePasswordTest_Incorrect_Password()
        {
            var data = new Dictionary<string, string>
            {
                { "ApplicationUserId", "1" },
                { "OldPassword", "wrong-password" },
                { "NewPassword", "password1" },
                { "ConfirmPassword", "password1" }
            };
            var response = await TestClient.PatchAsync(API_REQUESTS.UPDATE_PASSWORD, new FormUrlEncodedContent(data)).ConfigureAwait(false);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
    }
}