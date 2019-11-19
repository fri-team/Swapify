using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebAPI.Models.UserModels;

namespace IntegrationTest.ExchangeControllerTest
{
    public static class ExchangeControllerTestsData
    {
        public static StringContent CreateRegisterViewModel(string name, string surname, string email, string password, string passwordAgain)
        {
            RegisterModel model = new RegisterModel { Name = name, Surname = surname, Email = email, Password = password, PasswordAgain = passwordAgain };
            var jsonModel = JsonConvert.SerializeObject(model);
            return new StringContent(jsonModel, Encoding.UTF8, "application/json");
        }

        //public static HttpClient AuthenticateClient(HttpClient client)
        //{

        //}
    }
}
