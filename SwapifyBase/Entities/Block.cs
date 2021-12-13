using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using System;

namespace FRITeam.Swapify.SwapifyBase.Entities
{
    public class Block 
    {
        public Guid BlockId { get; set; }
        public BlockType BlockType { get; set; }
        public Guid CourseId { get; set; }
        public Day Day { get; set; }
        public byte StartHour { get; set; }
        public byte Duration { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }
        public string BlockColor { get; set; }

        public Block() { }

        public Block(BlockType type, Day day, byte startHour, byte duration, string room, string teacher)
        {
            BlockType = type;
            Day = day;
            StartHour = startHour;
            Duration = duration;
            Room = room;
            Teacher = teacher;
        }

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

        public Block Clone()
        {
            var newBlock = new Block
            {
                BlockId = this.BlockId,
                BlockType = this.BlockType,
                CourseId = this.CourseId,
                Day = this.Day,
                Duration = this.Duration,
                Room = this.Room,
                StartHour = this.StartHour,
                Teacher = this.Teacher
            };
            return newBlock;
        }        
        public bool SubjectIsAlreadyPresent(Block b)
        {
            return (this.Day == b?.Day) &&
                   ((this.StartHour >= b?.StartHour && this.StartHour <= b?.StartHour + b?.Duration) ||
                   (this.StartHour + this.Duration >= b?.StartHour && this.StartHour + this.Duration <= b?.StartHour + b?.Duration)) &&
                   (this.CourseId == b?.CourseId);
        }

        public override bool Equals(object obj)
        {
            return obj is Block block &&
                   BlockType == block.BlockType &&
                   CourseId.Equals(block.CourseId) &&
                   Day == block.Day &&
                   StartHour == block.StartHour &&
                   Duration == block.Duration &&
                   Room == block.Room &&
                   Teacher == block.Teacher;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BlockType, CourseId, Day, StartHour, Duration, Room, Teacher);
        }
    }
}
