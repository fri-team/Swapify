using BlazorClient.Models.Timetable;
using System;
using System.Threading.Tasks;
using BlazorClient.Models.Student;

namespace BlazorClient.Services
{
    public class TimetableService : ITimetableService
    {
        private TimetableModel _timetableModel;
        private ISwapifyAPI _swapifyAPI;
        public TimetableService(ISwapifyAPI swapifyAPI)
        {
            _swapifyAPI = swapifyAPI;
        }

        public TimetableModel Timetable
        {
            get => _timetableModel;
            set
            {
                _timetableModel = value;
                TimetableChanged?.Invoke(_timetableModel);
            }
        }

        public event Action<TimetableModel> TimetableChanged;

        public async Task<TimetableModel> SetTimetable(string email)
        {
            Timetable = await _swapifyAPI.Student.GetStudentTimetable(email);
            return Timetable;
        }

        public async Task<TimetableModel> SetTimetableByPersonalNumber(string email, string personalNumber)
        {
            Timetable = await _swapifyAPI.Timetable.SetTimetableFromPersonalNumber(new StudentModel() {
                Email = email,
                PersonalNumber = personalNumber
            });

            return Timetable;
        }
    }
}
