using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Student : BaseEntity
    {
        public List<Timetable> Timetables { get; set; }
        public List<BlockChangeRequest> Requests { get; set; }
        public StudyGroup StudyGroup { get; set; }

        public Student()
        {
            Timetables = new List<Timetable>();
            Requests = new List<BlockChangeRequest>();
        }
    }
}
