using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.SwapifyBase.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IBaseUserService
    {
        Task AddAsync(BaseUser entityToAdd);
        Task<BaseUser> FindByIdAsync(Guid guid);
        Task UpdateStudentAsync(BaseUser userToUpdate);
        Task UpdateStudentTimetableAsync(BaseUser userToUpdate, Timetable userTimetable);
    }
}
