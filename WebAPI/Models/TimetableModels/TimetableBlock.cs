using System;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;

namespace WebAPI.Models.TimetableModels
{
    public class TimetableBlock
    {
        public string Id { get; set; }
        public int Day { get; set; }
        public int StartBlock { get; set; }
        public int EndBlock { get; set; }
        public string CourseName { get; set; }
        public string CourseShortcut { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }
        public TimetableBlockType Type { get; set; }

        public static Block ConvertToBlock(TimetableBlock blockToConvert, Guid courseId)
        {
            Block block = new Block();
            block.CourseId = courseId;
            block.Day = (Day)blockToConvert.Day;
            block.StartHour = (byte)blockToConvert.StartBlock;
            block.Duration = (byte)(blockToConvert.EndBlock - blockToConvert.StartBlock);
            block.Room = blockToConvert.Room;
            block.Teacher = blockToConvert.Teacher;
            block.BlockType = (BlockType)blockToConvert.Type;

            return block;
        }
    }
}
