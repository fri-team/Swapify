using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleTimetableResult
    {
        public UnizaScheduleContentResult Result { get; set; }
        public Semester Semester { get; set; }        
    }
}
