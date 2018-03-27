using FRITeam.Swapify.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class BlockChangeRequest : BaseEntity
    {
        public List<Block> BlockOpitonsChange { get; set; }
        public Block BlockForChange { get; set; }
        public Student Student { get; set; }
        public eStatusType Status { get; set; }

        public BlockChangeRequest()
        {
            BlockOpitonsChange = new List<Block>();
        }
    }
}
