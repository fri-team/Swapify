using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Student : BaseEntity
    {
        public Timetable Timetable { get; set; }
        public List<Guid> Requests { get; set; }
        public Guid StudyGroupId { get; set; }

        public Student()
        {
            Requests = new List<Guid>();
        }
    }
}
