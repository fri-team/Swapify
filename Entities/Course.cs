using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Course : BaseEntity
    {
        public List<Timetable> Timetables { get; set; }

    }
}
