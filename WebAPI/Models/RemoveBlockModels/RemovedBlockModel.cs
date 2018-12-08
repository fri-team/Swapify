using System.ComponentModel.DataAnnotations;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;

namespace WebAPI.Models.RemoveBlockModel
{
    public class RemovedBlockModel
    {
        [Required(ErrorMessage = "Typ je povinný.")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Deň je povinný.")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Začiatočná hodina bloku je povinná.")]
        public int StartHour { get; set; }

        [Required(ErrorMessage = "Dĺžka bloku je povinná.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Miestnosť je povinná.")]
        public string Room { get; set; }
            
        [Required(ErrorMessage = "Meno učiteľa je povinné.")]
        public string Teacher { get; set; }


        public RemovedBlockModel() { }

        public static Block ConvertToBlock(RemovedBlockModel blockToConvert)
        {
            Block block = new Block();
            block.BlockType = (BlockType)blockToConvert.Type;
            block.Day = (Day)blockToConvert.Day;
            block.StartHour = (byte)blockToConvert.StartHour;
            block.Duration = (byte)blockToConvert.Duration;
            block.Room = blockToConvert.Room;
            block.Teacher = blockToConvert.Teacher;
            return block;
        }
    }
}



