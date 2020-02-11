
using FRITeam.Swapify.APIWrapper.Objects;
using System.Collections.Generic;

namespace FRITeam.Swapify.APIWrapper
{
    public interface ISchoolCourseProxy
    {
        IEnumerable<CourseContent> GetByCourseName(string courseName);
    }
}
