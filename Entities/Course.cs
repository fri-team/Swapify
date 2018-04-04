
namespace FRITeam.Swapify.Entities
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }
        public Timetable Timetables { get; set; }

    }
}
