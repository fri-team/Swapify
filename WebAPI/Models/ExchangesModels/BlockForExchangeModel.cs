using FRITeam.Swapify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Exchanges
{
    public class BlockForExchangeModel
    {
        public int Day { get; set; }
        public int Duration { get; set; }
        public int StartHour { get; set; }
        [Required(ErrorMessage = "CourseId musí byť zadané.")]
        public string CourseId { get; set; }

        public BlockForExchangeModel() { }
        public static Block ConvertToBlock(BlockForExchangeModel blockToConvert)
        {
            Block blc = new Block();
            blc.CourseId = Guid.Parse(blockToConvert.CourseId);
            blc.Day = (Day)blockToConvert.Day;
            blc.Duration = (byte)blockToConvert.Duration;
            blc.StartHour = (byte)blockToConvert.StartHour;
            return blc;
        }
    }
}
