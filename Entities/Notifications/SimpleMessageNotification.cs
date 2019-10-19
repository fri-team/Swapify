using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Entities.Notifications
{
    public class SimpleMessageNotification: Notification
    {
        public string Message { get; set; }
        public override string Text => Message;
    }
}
