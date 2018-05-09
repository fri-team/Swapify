using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class Timetable : BaseEntity
    {
        /// <summary>
        /// !!! Do not use this property from outside this class,
        /// public is just because of mongodb mapper !!
        /// </summary>
        public List<Block> _blocks { get; set; }

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
            foreach (var blck in _blocks)
            {
                if (blck.IsSameAs(bl))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
