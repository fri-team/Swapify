using BlazorClient.Models.Timetable;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.Models.UserModels;

namespace BlazorClient.Services.API
{
    public class StudentEndpoints : IStudentEndpoints
    {
        private readonly HttpClient _httpClient;

        public StudentEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddNewTimetableBlock(AddNewBlockModel addBlockModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(addBlockModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/student/addNewBlock", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<TimetableModel> GetStudentTimetable(string studentEmail)
        {
            return await _httpClient.GetJsonAsync<TimetableModel>("/api/student/getStudentTimetable/" + studentEmail);
        }
    }
}
