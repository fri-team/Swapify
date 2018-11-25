using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;

namespace FRITeam.Swapify.Backend.Emails
{
    public abstract class EmailBase
    {
        protected string OutputDirLocation { get; }
        protected abstract string PathToTemplate { get; }
        protected abstract string Subject { get; }
        protected MailAddress Sender { get; set; }
        protected MailAddress Receiver { get; set; }        
        protected string BaseUrl { get; set; }
        protected string Body { get; set; }

        protected EmailBase(string sender, string senderDisplayName, string receiver, string baseUrl)
        {
            Sender = new MailAddress(sender, senderDisplayName);
            Receiver = new MailAddress(receiver);
            OutputDirLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            BaseUrl = baseUrl;
        }
                
        public abstract MailMessage CreateMailMessage();
        protected abstract bool CreateEmailBody();

        protected Attachment CreateImgAttachment(string pathToAttachment, string contentId)
        {
            Attachment attachment = new Attachment(pathToAttachment);
            attachment.ContentId = contentId;
            attachment.ContentDisposition.Inline = true;
            attachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            return attachment;
        }
    }
}
