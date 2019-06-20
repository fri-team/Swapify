using System;
using FRITeam.Swapify.Entities.Enums;
using Newtonsoft.Json;

namespace FRITeam.Swapify.Entities.Notifications
{
    public abstract class Notification: BaseEntity
    {
        [JsonIgnore]
        public Guid RecipientId { get; set; }

        public NotificationType Type { get; set; }
        
        public abstract string Text { get; }        

        public bool Read { get; set; }                

        public DateTime CreatedAt { get; set; }        
    }
}
