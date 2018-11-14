using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Backend.Notification
{
    public interface IMailGunSettings
    {
        string ApiKey { get; }
        string SenderDomain { get; }
        string Domain { get; }
        string FromEmail { get; }
    }
}
