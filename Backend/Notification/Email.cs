using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Backend.Notification
{
    public class Email
    {
        public string FromEmail { get; set; } = "Mailgun Sandbox <postmaster @sandbox260b1e74a61b4b7faf43e42dd656e3a0.mailgun.org>";
        public string ToEmail { get; set; } = "Dávid<madaras.david1@gmail.com>"
        public string Body { get; set; }
        public string Subject { get; set; }


    }
}
