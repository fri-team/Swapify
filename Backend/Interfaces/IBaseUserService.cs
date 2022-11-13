using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.SwapifyBase.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface IBaseUserService
    {
        Task AddAsync(UserData entityToAdd);
        Task<UserData> FindByIdAsync(Guid guid);
        Task UpdateStudentAsync(UserData userToUpdate);
        Task UpdateStudentTimetableAsync(UserData userToUpdate, Timetable userTimetable);
    }
}
