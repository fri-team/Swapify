using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class StudyGroup : BaseEntity
    {
        public List<Course> Courses { get; set; }
        public List<Timetable> Timetables { get; set; }

    }
}
