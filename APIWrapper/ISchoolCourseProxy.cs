using FRITeam.Swapify.APIWrapper.Objects;
using System.Threading.Tasks;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolCourseProxy
    {
        Task<UnizaCourseContentResult> GetByCourseName(string courseName);
    }
}
