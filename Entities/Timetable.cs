using FRITeam.Swapify.Entities.Enums;
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
                block.BlockId = Guid.NewGuid();
                newTimetable.AddNewBlock(block.Clone());
            }

            return newTimetable;
        }

        public Block GetBlock(Guid blockId)
        {
            foreach (Block block in _blocks)
            {
                if (block.BlockId == blockId)
                {
                    return block;
                }
            }
            return null;
        }

        public Block GetBlock(Block block)
        {
            foreach (Block bl in _blocks)
            {
                if (bl.IsSameAs(block))
                    return bl;
            }
            return null;
        }

        public void AddNewBlock(Block newBlock)
        {
            if (newBlock.BlockId == null || newBlock.BlockId == Guid.Empty)
            {
                newBlock.BlockId = Guid.NewGuid();
            }
            _blocks.Add(newBlock);
        }

        public bool RemoveBlock(Guid blockId)
        {
            foreach (Block block in _blocks)
            {
                if (block.BlockId == blockId)
                {
                    _blocks.Remove(block);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateBlock(Block newBlock)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].BlockId == newBlock.BlockId)
                {
                    _blocks[i] = newBlock;
                    return true;
                }
            }
            return false;
        }

        public void UpdateColorOfBlocksWithSameCourseId(Block block)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].CourseId == block.CourseId)
                {
                    _blocks[i].BlockColor = block.BlockColor;
                }
            }
        }

        public bool ContainsBlock(Block bl)
        {
            return _blocks.Any(x => x.IsSameAs(bl));
        }

        public bool IsSubjectPresentInTimetable(Block bl)
        {
            return _blocks.Any(x => x.SubjectIsAlreadyPresent(bl));
        }

    }
}
