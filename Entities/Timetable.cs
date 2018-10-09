using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Timetable : BaseEntity
    {
        private List<Block> _blocks;
        
        public virtual IList<Block> AllBlocks
        {
            get => _blocks.AsReadOnly();
        }

        public Timetable()
        {
            _blocks = new List<Block>();
        }

        public Block GetBlock(Guid blockId)
        {
            return _blocks.FirstOrDefault(x => x.Id == blockId);
        }

        public void AddNewBlock(Block newBlock)
        {
            newBlock.Id = Guid.NewGuid();
            _blocks.Add(newBlock);
        }

        public bool RemoveBlock(Guid blockId)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].Id.Equals(blockId))
                {
                    _blocks.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool ContainsBlock(Block bl)
        {
            return _blocks.Any(x => x.IsSameAs(bl));
        }

        public bool UpdateBlock(Block block)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].Id.Equals(block.Id))
                {
                    _blocks[i] = block;
                    return true;
                }
            }
            return false;
        }
    }
}
