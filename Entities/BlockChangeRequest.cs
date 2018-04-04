using FRITeam.Swapify.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class BlockChangeRequest : BaseEntity
    {
        public List<Guid> BlockOptionsChange { get; set; }
        public Guid BlockForChangeId { get; set; }
        public Guid StudentId { get; set; }
        public StatusRequestType Status { get; set; }

        public BlockChangeRequest()
        {
            BlockOptionsChange = new List<Guid>();
        }
    }
}
