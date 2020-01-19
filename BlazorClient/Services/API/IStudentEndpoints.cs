using BlazorClient.Models.Timetable;
using System.Threading.Tasks;
using WebAPI.Models.UserModels;

namespace BlazorClient.Services.API
{
    public interface IStudentEndpoints
    {
        Task<TimetableModel> GetStudentTimetable(string studentEmail);
        Task<bool> AddNewTimetableBlock(AddNewBlockModel addblockModel);
    }
}
