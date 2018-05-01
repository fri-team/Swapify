using FRITeam.Swapify.Entities.Enums;
using System;

namespace FRITeam.Swapify.Entities
{
    public class Block : BaseEntity
    {
        public BlockType BlockType { get; set; }
        public Guid CourseId { get; set; }
        public Day Day { get; set; }
        public byte StartHour { get; set; }
        public byte Duration { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }

        public bool IsSameAs(Block b)
        {
            return (this.BlockType == b?.BlockType) &&
                (this.Day == b?.Day) &&
                (this.StartHour == b?.StartHour) &&
                (this.Duration == b?.Duration) &&
                (this.Room == b?.Room) &&
                (this.Teacher == b?.Teacher);
        }
    }
}
