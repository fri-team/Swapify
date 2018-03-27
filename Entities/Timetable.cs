using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Timetable: BaseEntity
    {
        public List<Block> Blocks { get; set; }

        public Timetable()
        {
            Blocks = new List<Block>();
        }
    }
}
