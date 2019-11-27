using System.Net.Http;
using System.Threading.Tasks;
using BlazorClient.Models.UserModels;
using Microsoft.AspNetCore.Components;
using System.Text.Json;


namespace BlazorClient.Services.API
{
    public class UserEndpoints: IUserEndpoints
    {
        private readonly HttpClient _httpClient;

        public UserEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthenticatedUserModel> Login(LoginModel loginModel)
        {
            return await _httpClient.PostJsonAsync<AuthenticatedUserModel>("/api/user/login", loginModel);
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(registerModel), System.Text.Encoding.UTF8, "application/json");            
            var response = await _httpClient.PostAsync("/api/user/register", content);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
