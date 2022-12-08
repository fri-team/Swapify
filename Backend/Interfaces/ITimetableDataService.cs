using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface ITimetableDataService
    {
        Task AddAsync(TimetableData entityToAdd);
        Task<TimetableData> FindByIdAsync(Guid guid);
        Task UpdateTimetableDataAsync(TimetableData timetableDataToUpdate);
        Task UpdateTimetableAsync(TimetableData timeTableDataToUpdate, Timetable newTimetable);
        TimetableType GetTimetableTypeByPersonalNumber(string personalNumber);
    }
}
