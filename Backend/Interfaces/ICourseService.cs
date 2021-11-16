using FRITeam.Swapify.SwapifyBase.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface ICourseService
    {
        Task AddAsync(Course entityToAdd);
        Task<Course> FindByIdAsync(Guid guid);
        Task<Course> FindByCodeAsync(string code);
        Task<Course> FindByNameAsync(string name);
        List<Course> FindByStartName(string courseStartsWith, string personalNumber);
        Task<Course> GetOrAddNotExistsCourse(string courseCode, string courseName);        
        Task<string> FindCourseCodeFromProxy(Course course);        
        Task<Course> FindCourseTimetableFromProxy(Guid guid);
        Task<Course> FindCourseTimetableFromProxy(Course course);
        Task UpdateAsync(Course course);
    }
}
