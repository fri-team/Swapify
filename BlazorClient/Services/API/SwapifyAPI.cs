using System.Net.Http;

namespace BlazorClient.Services.API
{
    public class SwapifyApi : ISwapifyAPI
    {
        HttpClient _httpClient;
        public SwapifyApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
            User = new UserEndpoints(httpClient);
            Student = new StudentEndpoints(httpClient);
            Timetable = new TimetableEndpoints(httpClient);
        }

        public IUserEndpoints User { get; }
        public IStudentEndpoints Student { get; }
        public ITimetableEndpoints Timetable { get; }

        public void SetAuthorizationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        public void DeleteAuthorizationToken()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
