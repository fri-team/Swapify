using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IStudentNumberService
    {
        Task AddAsync(StudentNumber entityToAdd);
        Task<StudentNumber> FindByIdAsync(Guid guid);
        Task<StudentNumber> GetStudentNumberAsync(string studentNumber);
    }
}
