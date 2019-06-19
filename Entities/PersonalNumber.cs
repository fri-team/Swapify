using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class PersonalNumber : BaseEntity
    {
        public string Number { get; set; }
        public List<Guid> Courses { get; set; }
        public Timetable Timetable { get; set; }


        public PersonalNumber()
        {
            Courses = new List<Guid>();
        }
    }
}
