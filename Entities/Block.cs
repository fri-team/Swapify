using FRITeam.Swapify.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Block : BaseEntity
    {
        public eBlockType BlockType{ get; set; }

        public List<TimeSlot> TimeSlots { get; set; }

        public Course Course { get; set; }

    }
}
