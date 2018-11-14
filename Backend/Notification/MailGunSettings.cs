using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Backend.Notification
{
    public class MailGunSettings : IMailGunSettings
    {
        public string ApiKey { get; }
        public string SenderDomain { get; }
        public string Domain { get; }
        public string FromEmail { get; }

        public MailGunSettings(string apikey,string senderDomain, string domain,string fromEmail)
        {
            this.FromEmail = fromEmail;
            this.ApiKey = apikey;
            this.Domain = domain;
            this.SenderDomain = senderDomain;
        }
    }
}
