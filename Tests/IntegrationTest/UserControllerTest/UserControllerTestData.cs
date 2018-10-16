using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebAPI.Models;

namespace IntegrationTest.UserControllerTest
{
    public static class UserControllerTestData
    {
        public static StringContent CreateRegisterViewModel(string name, string surname, string email, string password, string passwordAgain)
        {
            RegisterViewModel model = new RegisterViewModel { Name = name, Surname = surname, Email = email, Password = password, PasswordAgain = passwordAgain };
            var jsonModel = JsonConvert.SerializeObject(model);
            return new StringContent(jsonModel, Encoding.UTF8, "application/json");
        }

        public static IEnumerable<object[]> Register_RegisterUser_ReturnsModelStateErrors_Data()
        {
            // Empty name => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("", "Testovaci", "tester@testovaciii.com", "Heslo.123", "Heslo.123")
            };

            // Empty surname => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "", "tester@testovaciii.com", "Heslo.123", "Heslo.123")
            };

            // Empty email => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "Testovaci", "", "Heslo.123", "Heslo.123")
            };

            // Empty password => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "Testovaci", "tester@testovaciii.com", "", "Heslo.123")
            };

            // Empty passwordAgain => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "Testovaci", "tester@testovaciii.com", "Heslo.123", "")
            };

            // Invalid email => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "Testovaci", "tester.testovaciii.com", "Heslo.123", "Heslo.123")
            };

            // Short password => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "Testovaci", "tester@testovaciii.com", "Heslo", "Heslo")
            };

            // Passwords do not match => expected result is BadRequest
            yield return new object[]
            {
                CreateRegisterViewModel("Tester", "Testovaci", "tester@testovaciii.com", "Heslo.123", "Heslo123")
            };            
        }
    }
}
