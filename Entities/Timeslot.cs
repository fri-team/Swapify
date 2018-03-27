using FRITeam.Swapify.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class TimeSlot : BaseEntity
    {
        public eDay Day { get; set; }
        public int StartHour { get; set; }
    }
}
