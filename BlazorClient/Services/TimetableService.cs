using BlazorClient.Models.Student;
using BlazorClient.Models.Timetable;
using System;
using System.Threading.Tasks;
using BlazorClient.Services.IdentityManagement;
using WebAPI.Models.UserModels;
using BlazorClient.Models.UserModels;

namespace BlazorClient.Services
{
    public class TimetableService : ITimetableService
    {
        private TimetableModel _timetableModel;
        private ISwapifyAPI _swapifyAPI;
        private IUserService _userService;

        public TimetableService(ISwapifyAPI swapifyAPI, IUserService userService)
        {
            _swapifyAPI = swapifyAPI;
            _userService = userService;
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

        public async Task<bool> AddNewTimetableBlock(TimetableBlockModel newBlock)
        {            
            var authenticatedUser = await _userService.GetAuthenticatedUserAsync();

            var addNewBlockModel = new AddNewBlockModel
            {
                TimetableBlock = newBlock,
                User = new User()
                {
                    Email = authenticatedUser.Email,
                    Name = authenticatedUser.Name,
                    Surname = authenticatedUser.Surname,
                    UserName = authenticatedUser.UserName
                }
            };

            return await _swapifyAPI.Student.AddNewTimetableBlock(addNewBlockModel);
        }

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
