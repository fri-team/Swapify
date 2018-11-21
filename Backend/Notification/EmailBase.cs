using System.Net.Mail;
using System.Net.Mime;

namespace FRITeam.Swapify.Backend.Notification
{
    public class EmailBase
    {
        private const string PathToMac = "EmailTemplate/images/home -background.png";
        private const string PathToLogo = "EmailTemplate/images/swapify.png";
        private const string MacContentId = "Mac";
        private const string LogoContentId = "Swapify";

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
                Attachment mac = new Attachment(PathToMac);
                message.Attachments.Add(mac);
                string contentID = MacContentId;
                mac.ContentId = contentID;

                Attachment swapify = new Attachment(PathToLogo);
                message.Attachments.Add(swapify);
                string contentID2 = LogoContentId;
                swapify.ContentId = contentID2;

                swapify.ContentDisposition.Inline = true;
                swapify.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                mac.ContentDisposition.Inline = true;
                mac.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            }
        }
    }
}
