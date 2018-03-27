using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }
        public List<Timetable> Timetables { get; set; }

        public Course()
        {
            Timetables = new List<Timetable>();
        }
    }
}
