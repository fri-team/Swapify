using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Backend.Notification
{
    public interface IEmailSender
    {
        void SendSimpleMessage(Email email);
    }
}
