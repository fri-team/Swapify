using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IStudentService
    {
        Task AddAsync(Student entityToAdd);
        Task<Student> FindByIdAsync(Guid guid);
        Task UpdateStudentAsync(Student studentToUpdate);
        Task UpdateStudentTimetableAsync(Student studentToUpdate, Timetable studentTimetable);
    }
}
