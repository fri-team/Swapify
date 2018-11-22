using Mustache;
using System.IO;
using System.Net.Mail;

namespace FRITeam.Swapify.Backend.Emails.ConfirmationEmails
{
    public abstract class ConfirmationEmailBase : EmailBase
    {
        protected const string PathToMac = @"Emails/EmailTemplates/images/home-background.png";
        protected const string PathToLogo = @"Emails/EmailTemplates/images/swapify.png";
        protected const string MacContentId = "Mac";
        protected const string LogoContentId = "Swapify";
        protected const string MacImg = "cid:" + MacContentId;
        protected const string LogoImg = "cid:" + LogoContentId;

        protected ConfirmationEmailBase(string sender, string senderDisplayName, string receiver)
            : base(sender, senderDisplayName, receiver)
        {
        }

        public virtual void CreateConfirmationEmailBody(string baseUrl, string confirmationLink)
        {
            string path = Path.Combine(Location, PathToTemplate);
            Body = File.ReadAllText(path);
            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(Body);
            Body = generator.Render(new
            {
                link = baseUrl,
                confirmationLink = confirmationLink,
                img1 = MacImg,
                img2 = LogoImg
            });
        }

        public override MailMessage CreateMailMessage()
        {
            MailMessage mailMessage = new MailMessage(Sender, Receiver);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.Sender = Sender;
            mailMessage.IsBodyHtml = true;

            Attachment macImg = CreateAttachment(Path.Combine(Location, PathToMac), MacContentId);
            mailMessage.Attachments.Add(macImg);
            Attachment swapifyImg = CreateAttachment(Path.Combine(Location, PathToLogo), LogoContentId);
            mailMessage.Attachments.Add(swapifyImg);

            return mailMessage;
        }
    }
}
