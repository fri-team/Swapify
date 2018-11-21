using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mustache;

namespace FRITeam.Swapify.Backend.Notification
{
    public class EmailBase
    {
        private const string pathToMac = "EmailTemplate/images/home -background.png";
        private const string pathToLogo = "EmailTemplate/images/swapify.png";
        private const string macContentID = "Mac";
        private const string logoContentID = "Swapify";

        public string FromEmail { get; set; } = "Mailgun Sandbox <postmaster @sandbox260b1e74a61b4b7faf43e42dd656e3a0.mailgun.org>";
        public string ToEmail { get; set; } = "ExampleName<youremail@gmail.com>";
        public string Body { get; set; }
        public string Subject { get; set; }

        public EmailBase()
        {
        }

        public void AddAttachmentToTemplate(MailMessage message)
        {
            using (message)
            {
                Attachment mac = new Attachment(pathToMac);
                message.Attachments.Add(mac);
                string contentID = macContentID;
                mac.ContentId = contentID;

                Attachment swapify = new Attachment(pathToLogo);
                message.Attachments.Add(swapify);
                string contentID2 = logoContentID;
                swapify.ContentId = contentID2;

                swapify.ContentDisposition.Inline = true;
                swapify.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                mac.ContentDisposition.Inline = true;
                mac.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            }
        }
    }
}
