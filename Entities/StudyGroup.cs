using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class StudyGroup : BaseEntity
    {
        public string  GroupName { get; set; }
        public List<Guid> Courses { get; set; }
        public List<Timetable> Timetables { get; set; }


        public StudyGroup()
        {
            Courses = new List<Guid>();
            Timetables = new List<Timetable>();
        }
    }
}
