using BlazorClient.Models.Timetable;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorClient.Services.API
{
    public class StudentEndpoints : IStudentEndpoints
    {
        private readonly HttpClient _httpClient;

        public StudentEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TimetableModel> GetStudentTimetable(string studentEmail)
        {
            return await _httpClient.GetJsonAsync<TimetableModel>("/api/student/getStudentTimetable/" + studentEmail);
        }
    }
}
