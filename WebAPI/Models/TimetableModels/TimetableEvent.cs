using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using System;

namespace WebAPI.Models.TimetableModels
{
    public class TimetableEvent
    {
        public string CourseName { get; set; }
        public string Room { get; set; }
        public int StartBlock { get; set; }
        public int EndBlock { get; set; }
        public string BlockColor { get; set; }
        public TimetableBlockType Type { get; set; }
        public string Id { get; set; }
        public int Day { get; set; }
        public string CourseId { get; set; }

        public string CourseCode { get; set; }
        public string CourseShortcut { get; set; }

 
      
        public static Block ConvertToBlock(TimetableEvent blockToConvert, Guid courseId)
        {
            Block block = new Block();
            if (blockToConvert.Id != null)
            {
                block.BlockId = Guid.Parse(blockToConvert.Id);
            }
            block.CourseId = courseId;
            block.Day = (Day)blockToConvert.Day;
            block.StartHour = (byte)blockToConvert.StartBlock;
            block.Duration = (byte)(blockToConvert.EndBlock - blockToConvert.StartBlock);
            block.Room = blockToConvert.Room;
            block.BlockType = (BlockType)blockToConvert.Type;
            block.BlockColor = blockToConvert.BlockColor;
            return block;
        }
    }
}
