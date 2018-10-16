using FRITeam.Swapify.Entities.Enums;
using System;

namespace FRITeam.Swapify.Entities
{
    public class Block 
    {
        public BlockType BlockType { get; set; }
        public Guid CourseId { get; set; }
        public Day Day { get; set; }
        public byte StartHour { get; set; }
        public byte Duration { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }

        /// <summary>
        /// Doesnt compare course ID
        /// </summary>
        public bool IsSameAs(Block b)
        {
            return (this.BlockType == b?.BlockType) &&
                (this.Day == b?.Day) &&
                (this.StartHour == b?.StartHour) &&
                (this.Duration == b?.Duration) &&
                (this.Room == b?.Room) &&
                (this.Teacher == b?.Teacher);
        }

        public override bool Equals(object obj)
        {
            Block other = obj as Block;
            return (IsSameAs(other) && this.CourseId == other.CourseId);
        }
    }
}
