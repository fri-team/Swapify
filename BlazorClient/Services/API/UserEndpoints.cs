using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorClient.Models.UserModels;
using Microsoft.AspNetCore.Components;
using WebAPI.Models.UserModels;

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
    }
}
