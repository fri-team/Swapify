using System;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;

namespace WebAPI.Models.RemoveBlockModel
{
    public class RemovedBlockModel
    {
        public int Type { get; set; }
        public int Day { get; set; }
        public int StartHour { get; set; }
        public int Duration { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }


        public RemovedBlockModel() { }

        public static Block ConvertToBlock(RemovedBlockModel blockToConvert)
        {
            Block block = new Block();
            block.BlockType = (BlockType)blockToConvert.Type;
            block.Day = (Day)blockToConvert.Day;
            block.StartHour = (byte)blockToConvert.StartHour;
            block.Duration = (byte) blockToConvert.Duration;
            block.Room = blockToConvert.Room;
            block.Teacher = blockToConvert.Teacher;
            return block;
        }
    }
}



