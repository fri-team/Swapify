using BlazorClient.Models.Timetable;
using System;
using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public interface ITimetableService
    {
        TimetableModel Timetable { get; }
        event Action<TimetableModel> TimetableChanged;
        Task<TimetableModel> SetTimetableByPersonalNumber(string email, string personalNumber);
        Task<TimetableModel> SetTimetable(string email);
    }
}
