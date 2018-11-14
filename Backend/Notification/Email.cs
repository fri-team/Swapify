using System;
using System.Collections.Generic;
using System.Text;

namespace FRITeam.Swapify.Backend.Notification
{
    public class Email
    {
        public string ToEmail { get; set; } = "ExampleName<youremail@gmail.com>";
        public string Body { get; set; }
        public string Subject { get; set; }


    }
}
