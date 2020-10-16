using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface ICourseService
    {
        Task AddAsync(Course entityToAdd);
        Task<Course> FindByIdAsync(Guid guid);
        Task<Course> FindByNameAsync(string name);
        List<Course> FindByStartName(string courseStartsWith);
        Task<Course> GetOrAddNotExistsCourseByShortcut(string courseShortcut, string courseName = null);
        Task<Course> GetOrAddNotExistsCourseByName(string courseName, string courseCode);
        string FindCourseShortCutFromProxy(Course course);
        Task<Course> FindCourseTimetableFromProxy(Guid guid);
        Task<Course> FindCourseTimetableFromProxy(string shortCut, Course course);
    }
}
