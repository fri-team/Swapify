using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.UserControllerTest
{
    public class UserControllerTests : IClassFixture<TestFixture>
    {
        public TestFixture TestFixture { get; set; }

        public UserControllerTests(TestFixture testFixture)
        {
            TestFixture = testFixture;
        }

        [Fact]
        public async Task Register_RegisterUser_ReturnsOkAsync()
        {
            Uri registerUri = new Uri(TestFixture.BaseUrl, "user/register/");
            HttpClient client = TestFixture.CreateClient();
            HttpContent registerViewModel = UserControllerTestData.CreateRegisterViewModel("Tester", "Testovaci", "fri.t3am@gmail.com", "Heslo.123", "Heslo.123");
            HttpResponseMessage response = await client.PostAsync(registerUri, registerViewModel);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(UserControllerTestData.Register_RegisterUser_ReturnsModelStateErrors_Data), MemberType = typeof(UserControllerTestData))]
        public async Task Register_RegisterUser_ReturnsModelStateErrorsAsync(HttpContent registerViewModel)
        {
            Uri uri = new Uri(TestFixture.BaseUrl, "user/register/");
            HttpClient client = TestFixture.CreateClient();
            HttpResponseMessage response = await client.PostAsync(uri, registerViewModel);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_RegisterUserThatExists_ReturnsIdentityErrorsAsync()
        {
            Uri uri = new Uri(TestFixture.BaseUrl, "user/register/"); 
             HttpClient client = TestFixture.CreateClient();
            HttpContent registerViewModel = UserControllerTestData.CreateRegisterViewModel("Tester", "Testovaci", "frit3am@gmail.com", "Heslo.123", "Heslo.123");
            HttpResponseMessage response = await client.PostAsync(uri, registerViewModel);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            HttpResponseMessage response2 = await client.PostAsync(uri, registerViewModel);
            Assert.True(response2.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
