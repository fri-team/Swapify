using BlazorClient.Models.Student;
using BlazorClient.Models.Timetable;
using System;
using System.Threading.Tasks;
using BlazorClient.Services.IdentityManagement;

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

        public async Task<TimetableModel> LoadTimetable(string email)
        {
            Timetable = await _swapifyAPI.Student.GetStudentTimetable(email);
            return Timetable;
        }

        public async Task<TimetableModel> SetTimetableByPersonalNumber(string email, string personalNumber)
        {
            var ok = await _swapifyAPI.Timetable.SetTimetableFromPersonalNumber(new StudentModel()
            {
                Email = email,
                PersonalNumber = personalNumber
            });

            if (ok)
            {
                return await LoadTimetable(email);
            }

            return new TimetableModel();
        }
    }
}
