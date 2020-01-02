using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorClient.Services.API
{
    public class SwapifyApi: ISwapifyAPI
    {
        HttpClient _httpClient;
        public SwapifyApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
            User = new UserEndpoints(httpClient);
            Student = new StudentEndpoints(httpClient);
        }

        public IUserEndpoints User { get; }
        public IStudentEndpoints Student { get; }

        public void SetAuthorizationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }
    }
}
