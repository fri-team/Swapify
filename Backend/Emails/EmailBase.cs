using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;

namespace FRITeam.Swapify.Backend.Emails
{
    public abstract class EmailBase
    {
        protected string Location { get; }
        protected abstract string PathToTemplate { get; }
        public abstract string Subject { get; }
        public MailAddress Sender { get; set; }
        public MailAddress Receiver { get; set; }
        public string Body { get; set; }

        protected EmailBase(string sender, string senderDisplayName, string receiver)
        {
            Sender = new MailAddress(sender, senderDisplayName);
            Receiver = new MailAddress(receiver);
            Location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public abstract MailMessage CreateMailMessage();

        protected Attachment CreateAttachment(string pathToAttachment, string contentId)
        {
            Attachment attachment = new Attachment(pathToAttachment);
            attachment.ContentId = contentId;
            attachment.ContentDisposition.Inline = true;
            attachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            return attachment;
        }
    }
}
