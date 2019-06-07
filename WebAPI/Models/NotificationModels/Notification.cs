using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.NotificationModels
{
    public class Notification
    {
        public Guid NotificationId { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }

        //public string Photo { get; set; }

        public bool Read { get; set; }

        // public User Sender { get; set; }

        //public IEnumerable<User> Recipients { get; set; }

        public DateTime CreatedAt { get; set; }        
    }
}
