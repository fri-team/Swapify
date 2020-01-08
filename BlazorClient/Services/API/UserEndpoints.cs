using BlazorClient.Models.UserModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System;

namespace BlazorClient.Services.API
{
    public class UserEndpoints : IUserEndpoints
    {
        private readonly HttpClient _httpClient;

        public UserEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginModel), System.Text.Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine("login");
                var response = await _httpClient.PostAsync("/api/user/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var authenticatedUserModel = JsonSerializer.Deserialize<AuthenticatedUserModel>(await response.Content.ReadAsStringAsync());
                    return new LoginResult()
                    {
                        AuthenticatedUser = authenticatedUserModel,
                        Successful = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync());
                    errorResult.Successful = false;
                    return errorResult;
                }

                return new LoginResult()
                {
                    Successful = false,
                    Error = "Nastala chyba."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new LoginResult()
                {
                    Successful = false,
                    Error = "chyba"
                };
            }                        
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(registerModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/user/register", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(resetPasswordModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/user/resetPassword", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetNewPassword(SetNewPasswordModel setNewPasswordModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(setNewPasswordModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/user/setNewPassword", content);

            return response.IsSuccessStatusCode;
        }
    }
}
