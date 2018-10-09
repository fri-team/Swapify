using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool RemoveBlock(Guid blockId)
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (Blocks[i].Id.Equals(blockId))
                {
                    Blocks.RemoveAt(i);
                    return true;
                }
            }
            return false;            
        }

        public bool ContainsBlock(Block bl)
        {
            foreach (var blck in Blocks)
            {
                if(blck.IsSameAs(bl))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateBlock(Block block)
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (Blocks[i].Id.Equals(block.Id))
                {
                    Blocks[i] = block;
                    return true;
                }
            }
            return false;
        }
    }
}
