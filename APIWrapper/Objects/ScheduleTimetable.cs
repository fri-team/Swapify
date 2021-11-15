using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleTimetable
    {
        public IEnumerable<ScheduleContent> ScheduleHourContents { get; set; }
        public Semester Semester { get; set; }
    }
}
