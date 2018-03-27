using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class StudyGroup : BaseEntity
    {
        public string  GroupName { get; set; }
        public List<Course> Courses { get; set; }
        public List<Timetable> Timetables { get; set; }


        public StudyGroup()
        {
            Courses = new List<Course>();
            Timetables = new List<Timetable>();
        }
    }
}
