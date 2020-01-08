using BlazorClient.Models.Student;
using System.Threading.Tasks;

namespace BlazorClient.Services.API
{
    public interface ITimetableEndpoints
    {
        Task<bool> SetTimetableFromPersonalNumber(StudentModel studentModel);
    }
}
