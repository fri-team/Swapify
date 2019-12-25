using BlazorClient.Models.Timetable;
using System.Threading.Tasks;

namespace BlazorClient.Services.API
{
    public interface IStudentEndpoints
    {
        Task<TimetableModel> GetStudentTimetable(string studentEmail);
    }
}
