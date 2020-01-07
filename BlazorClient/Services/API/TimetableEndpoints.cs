using BlazorClient.Models.Student;
using BlazorClient.Models.Timetable;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Services.API
{
    public class TimetableEndpoints : ITimetableEndpoints
    {
        private HttpClient _httpClient;

        public TimetableEndpoints(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TimetableModel> SetTimetableFromPersonalNumber(StudentModel studentModel)
        {
            return await _httpClient.PostJsonAsync<TimetableModel>(
                "/api/student/setStudentTimetableFromPersonalNumber", studentModel);
        }
    }
}
