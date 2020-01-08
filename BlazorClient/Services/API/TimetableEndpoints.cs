using BlazorClient.Models.Student;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorClient.Services.API
{
    public class TimetableEndpoints : ITimetableEndpoints
    {
        private HttpClient _httpClient;

        public TimetableEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> SetTimetableFromPersonalNumber(StudentModel studentModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(studentModel), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/timetable/setStudentTimetableFromPersonalNumber", content);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
