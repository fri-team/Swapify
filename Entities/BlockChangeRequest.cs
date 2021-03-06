using FRITeam.Swapify.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities
{
    public class BlockChangeRequest : BaseEntity
    {
        /// <summary>
        /// Student who wants to change block
        /// </summary>
        public Guid StudentId { get; set; }
        public Block BlockFrom { get; set; }
        public Block BlockTo { get; set; }
        public ExchangeStatus Status { get; set; }
        public DateTime DateOfCreation { get; set; }

        public BlockChangeRequest()
        {
            Status = ExchangeStatus.WaitingForExchange;
        }
    }
}
