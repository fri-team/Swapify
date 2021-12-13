using System;
using System.ComponentModel.DataAnnotations;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;

namespace WebAPI.Models.Exchanges
{
    public class BlockForExchangeModel
    {
        public string BlockId { get; set; }
        public int Day { get; set; }
        public int Duration { get; set; }
        public int StartHour { get; set; }
        [Required(ErrorMessage = "CourseId musí byť zadané.")]
        public string CourseId { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }

        public BlockForExchangeModel() { }
        public static Block ConvertToBlock(BlockForExchangeModel blockToConvert)
        {
            Block blc = new Block();
            blc.BlockId = Guid.Parse(blockToConvert.BlockId);
            blc.CourseId = Guid.Parse(blockToConvert.CourseId);
            blc.Day = (Day)blockToConvert.Day;
            blc.Duration = (byte)blockToConvert.Duration;
            blc.StartHour = (byte)blockToConvert.StartHour;
            blc.Room = blockToConvert.Room;
            blc.Teacher = blockToConvert.Teacher;
            return blc;
        }
    }
}
