using System.Threading.Tasks;
using BlazorClient.Models.Timetable;
using BlazorClient.Models.Student;

namespace BlazorClient.Services.API
{
    public interface ITimetableEndpoints
    {
        Task<TimetableModel> SetTimetableFromPersonalNumber(StudentModel studentModel);
    }
}
