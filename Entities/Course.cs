
namespace FRITeam.Swapify.Entities
{
    public class Course : BaseEntity
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public Timetable Timetable { get; set; }

    }
}
