using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.UserControllerTest
{
    public class UserControllerTests : IClassFixture<TestFixture>
    {
        public TestFixture TestFixture { get; set; }
        private readonly Uri _registerUri;

        public UserControllerTests(TestFixture testFixture)
        {
            TestFixture = testFixture;
            _registerUri = new Uri(TestFixture.BaseUrl, "user/register/");
        }

        [Fact]
        public async Task Register_RegisterUser_ReturnsOkAsync()
        {
            HttpClient client = TestFixture.CreateClient();
            HttpContent registerViewModel = UserControllerTestData.CreateRegisterViewModel("Tester", "Testovaci", "fri.t3am@gmail.com", "Heslo.123", "Heslo.123");
            HttpResponseMessage response = await client.PostAsync(_registerUri, registerViewModel);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(UserControllerTestData.Register_RegisterUser_ReturnsModelStateErrors_Data), MemberType = typeof(UserControllerTestData))]
        public async Task Register_RegisterUser_ReturnsModelStateErrorsAsync(HttpContent registerViewModel)
        {
            HttpClient client = TestFixture.CreateClient();
            HttpResponseMessage response = await client.PostAsync(_registerUri, registerViewModel);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_RegisterUserThatExists_ReturnsIdentityErrorsAsync()
        {
            HttpClient client = TestFixture.CreateClient();
            HttpContent registerViewModel = UserControllerTestData.CreateRegisterViewModel("Tester", "Testovaci", "frit3am@gmail.com", "Heslo.123", "Heslo.123");
            HttpResponseMessage response = await client.PostAsync(_registerUri, registerViewModel);
            HttpResponseMessage response2 = await client.PostAsync(_registerUri, registerViewModel);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response2.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
