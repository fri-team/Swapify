
using System;

namespace FRITeam.Swapify.Entities
{
    public class Course : BaseEntity
    {
        public string CourseCode { get; set; }        
        public string CourseName { get; set; }
        public Timetable Timetable { get; set; }
        public bool IsLoaded { get; set; }
        public DateTime? LastUpdateOfTimetable { get; set; }
    }
}
