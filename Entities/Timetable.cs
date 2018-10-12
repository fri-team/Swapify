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

        public Timetable Clone()
        {
            var newTimetable = new Timetable();
            foreach (var block in _blocks)
            {
                newTimetable.AddNewBlock(block.Clone());
            }

            return newTimetable;
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

        public void DeleteBlock(Guid blockId)
        {
            var blc = _blocks.Find(x => x.Id == blockId);
            if (blc == null)
            {
                throw new ArgumentException($"Block with id {blockId} is not in collection.");
            }

            _blocks.Remove(blc);
        }

        public bool ContainsBlock(Block bl)
        {
            return _blocks.Any(x => x.IsSameAs(bl));
        }
    }
}
