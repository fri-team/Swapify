using FRITeam.Swapify.Entities.Enums;
using System;

namespace FRITeam.Swapify.Entities
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
            var newBlock = new Block();
            newBlock.BlockId = this.BlockId;
            newBlock.BlockType = this.BlockType;
            newBlock.CourseId = this.CourseId;
            newBlock.Day = this.Day;
            newBlock.Duration = this.Duration;
            newBlock.Room = this.Room;
            newBlock.StartHour = this.StartHour;
            newBlock.Teacher = this.Teacher;
            return newBlock;
        }

        public override bool Equals(object obj)
        {
            Block other = obj as Block;
            return (IsSameAs(other) && this.CourseId == other.CourseId);
        }

        public bool SubjectIsAlreadyPresent(Block b)
        {
            return (this.Day == b?.Day) &&
                   ((this.StartHour >= b?.StartHour && this.StartHour <= b?.StartHour + b?.Duration) ||
                   (this.StartHour + this.Duration >= b?.StartHour && this.StartHour + this.Duration <= b?.StartHour + b?.Duration)) &&
                   (this.CourseId == b?.CourseId);
        }

    }
}
