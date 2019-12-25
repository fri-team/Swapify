using BlazorClient.Models.Timetable;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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
