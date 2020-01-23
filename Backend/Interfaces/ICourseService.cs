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
        Task<Guid> GetOrAddNotExistsCourseIdByShortcut(string courseShortcut, Block courseBlock);
        Task<Guid> GetOrAddNotExistsCourseIdByName(string courseName, Block courseBlock);
        Task<Course> FindCourseTimetableFromProxy(Guid guid);
    }
}
