using BlazorClient.Models.Timetable;
using BlazorClient.Models.UserModels;

namespace WebAPI.Models.UserModels
{
    public class AddNewBlockModel
    {        
        public User User { get; set; }        
        public TimetableBlockModel TimetableBlock { get; set; }
    }
}
