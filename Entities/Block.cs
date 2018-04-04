using FRITeam.Swapify.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Block : BaseEntity
    {
        public BlockType BlockType { get; set; }
        public Guid CourseId { get; set; }
        public Day Day { get; set; }
        public int StartHour { get; set; }
        public int Duration { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }
    }
}
