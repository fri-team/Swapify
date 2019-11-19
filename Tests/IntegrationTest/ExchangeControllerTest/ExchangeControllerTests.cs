using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.Models.Exchanges;
using WebAPI.Models.UserModels;
using Xunit;

namespace IntegrationTest.ExchangeControllerTest
{
    public class ExchangeControllerTests : IClassFixture<TestFixture>
    {
        private static readonly Guid CourseGuid = Guid.Parse("180ce481-85a3-4246-93b5-ba0a0229c59f");
        private static readonly Guid Stduent1Guid = Guid.Parse("60030252-5873-4fe4-b32e-9c7e0d5e3517");
        private static readonly Guid Stduent2Guid = Guid.Parse("72338e48-9829-47b5-a666-766bbbecd799");

        public TestFixture TestFixture { get; set; }

        public ExchangeControllerTests(TestFixture testFixture)
        {
            TestFixture = testFixture;
        }

        //public async Task AuthenticateUserAsync(RegisterModel model)
        //{
        //    TestFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("tester", await GetJwtAsync(model));
        //}

        //private async Task<string> GetJwtAsync(RegisterModel model)
        //{
        //    var response = await TestFixture.Client.PostAsJsonAsync(new Uri(TestFixture.BaseUrl, "user/register/"), model);

        //    var registrationResponse = await response.Content.ReadAsAsync<string>();
        //    return registrationResponse;
        //}

        [Fact]
        public async Task ExchangeConfirm_Test()
        {
            Uri uri = new Uri(TestFixture.BaseUrl, "user/login/");
            Uri uriBlock = new Uri(TestFixture.BaseUrl, "student/getStudentTimetable/oleg@swapify.com/");

            HttpClient client = TestFixture.CreateClient();

            LoginModel rm = new LoginModel()
            {
                Email = "oleg@swapify.com",
                Password = "Heslo123"
            };

            var jsonModel = JsonConvert.SerializeObject(rm);
            StringContent content = new StringContent(jsonModel, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);

            string json = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(json);
            JToken token = o.SelectToken("$.token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.ToString());
            client.DefaultRequestHeaders.Add("Authorization", token.ToString());
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            //var u = new User("oleg@swapify.com", "Oleg", "Dementov");

            //jsonModel = JsonConvert.SerializeObject("oleg@swapify.com");
            //content = new StringContent(jsonModel, System.Text.Encoding.UTF8, "application/json");
            response = await client.GetAsync(uriBlock);

            Assert.True(response.IsSuccessStatusCode == true);
        }

        [Fact]
        public async Task Exchange_ExchangeConfirm_NonAuthorizedCall()
        {
            Uri uri = new Uri(TestFixture.BaseUrl, "exchange/ExchangeConfirm/");
            HttpClient client = TestFixture.CreateClient();

            ExchangeRequestModel exchange = new ExchangeRequestModel()
            {
                BlockFrom = new BlockForExchangeModel()
                {
                    Day = (int)Day.Monday,
                    Duration = 2,
                    StartHour = 9,
                    CourseId = CourseGuid.ToString()
                },
                BlockTo = new BlockForExchangeModel()
                {
                    Day = (int)Day.Thursday,
                    Duration = 2,
                    StartHour = 15,
                    CourseId = CourseGuid.ToString()
                },
                StudentId = Stduent1Guid.ToString()
            };

            var jsonModel = JsonConvert.SerializeObject(exchange);
            StringContent content = new StringContent(jsonModel, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);

            Assert.True(response.IsSuccessStatusCode == false);
        }
    }
}
