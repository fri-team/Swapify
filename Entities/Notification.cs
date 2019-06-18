using System;

namespace FRITeam.Swapify.Entities
{
    public class Notification: BaseEntity
    {        
        public Guid RecipientId { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }        

        public bool Read { get; set; }                

        public DateTime CreatedAt { get; set; }        
    }
}
