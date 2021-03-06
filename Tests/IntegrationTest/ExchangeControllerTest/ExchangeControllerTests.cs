using FRITeam.Swapify.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models.Exchanges;
using WebAPI.Models.UserModels;
using Xunit;

namespace IntegrationTest.ExchangeControllerTest
{
    public class ExchangeControllerTests : IClassFixture<TestFixture>
    {
        private readonly Uri LoginUri;
        private readonly Uri ExchangeUri;
        private readonly Uri WaitingExchangesUri;        

        public static TestFixture TestFixture { get; set; }

        public ExchangeControllerTests(TestFixture testFixture)
        {
            TestFixture = testFixture;
            LoginUri = new Uri(TestFixture.BaseUrl, "user/login/");
            ExchangeUri = new Uri(TestFixture.BaseUrl, "exchange/ExchangeConfirm/");
            WaitingExchangesUri = new Uri(TestFixture.BaseUrl, "exchange/userWaitingExchanges");
        }

        [Fact]
        public async Task Exchange_ExchangeConfirm_Successful()
        {
            // Arrange
            HttpClient client1 = TestFixture.CreateClient();
            var student1Id = await AuthenticateClient(client1, ExchangeControllerTestsData.Login1);
            HttpClient client2 = TestFixture.CreateClient();
            var student2Id = await AuthenticateClient(client2, ExchangeControllerTestsData.Login3);
            var waitingExchanges1Before = await GetUserWaitingExchanges(client1, student1Id);
            var waitingExchanges2Before = await GetUserWaitingExchanges(client2, student2Id);

            // Act
            var response11 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel11, client1, student1Id);
            var response12 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel12, client1, student1Id);
            var response13 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel13, client1, student1Id);

            var response21 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel21, client2, student2Id);
            var response22 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel22, client2, student2Id);

            var waitingExchanges1 = await GetUserWaitingExchanges(client1, student1Id);
            var waitingExchanges2 = await GetUserWaitingExchanges(client2, student2Id);

            // Assert
            Assert.True(response11.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response12.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response13.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response21.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response22.StatusCode == System.Net.HttpStatusCode.OK);
            // UserWaitingExchanges are not processed
            // Host doesn't make exchanges (count increases by every test run)

            //Assert.True(waitingExchanges1.Count == 1); 
            //Assert.True(waitingExchanges2.Count == 1);

            // Consider comparing state before the Act and after ?
            Assert.True(waitingExchanges1.Count == waitingExchanges1Before.Count + 1);
            Assert.True(waitingExchanges2.Count == waitingExchanges2Before.Count + 1);
            //TODO: Check if host makes exchanges 
        }

        [Fact]
        public async Task Exchange_ExchangeConfirm_NonAuthorizedCall()
        {
            // Arrange
            HttpClient client = TestFixture.CreateClient();

            // Act
            ExchangeControllerTestsData.ExchangeModel11.StudentId = ExchangeControllerTestsData.StduentGuid.ToString();
            var jsonModel = JsonConvert.SerializeObject(ExchangeControllerTestsData.ExchangeModel11);
            StringContent content = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ExchangeUri, content);

            // Assert
            Assert.True(response.IsSuccessStatusCode == false);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Exchange_ExchangeConfirm_UserNotFound()
        {
            // Arrange
            HttpClient client1 = TestFixture.CreateClient();
            var student1Id = await AuthenticateClient(client1, ExchangeControllerTestsData.Login1);
            HttpClient client2 = TestFixture.CreateClient();
            var student2Id = await AuthenticateClient(client2, ExchangeControllerTestsData.Login2);

            // Act
            var response12 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel12, client1, student1Id);
            var response22 = await SendExchangeRequest(ExchangeControllerTestsData.ExchangeModel22, client2, student2Id);

            // Assert
            Assert.True(response12.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response22.StatusCode == System.Net.HttpStatusCode.NotFound);
        }


        private async Task<string> AuthenticateClient(HttpClient client, LoginModel login)
        {
            var jsonModel = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(LoginUri, content);

            string json = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<AuthenticatedUserModel>(json);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userModel.Token);
            return userModel.StudentId;
        }

        private async Task<HttpResponseMessage> SendExchangeRequest(ExchangeRequestModel exchangeModel, HttpClient client, string studentId)
        {
            exchangeModel.StudentId = studentId;
            var jsonModel = JsonConvert.SerializeObject(exchangeModel);
            var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            return await client.PostAsync(ExchangeUri, content);
        }

        private async Task<List<BlockChangeRequest>> GetUserWaitingExchanges(HttpClient client, string studentId)
        { 
            var jsonModel = JsonConvert.SerializeObject(studentId);
            var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(WaitingExchangesUri, content);

            string json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<BlockChangeRequest>>(json);
            return list;
        }
    }
}
