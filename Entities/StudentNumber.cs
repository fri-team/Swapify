using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class StudentNumber : BaseEntity
    {
        public string Number { get; set; }
        public List<Guid> Courses { get; set; }
        public Timetable Timetable { get; set; }


        public StudentNumber()
        {
            Courses = new List<Guid>();
        }
    }
}
