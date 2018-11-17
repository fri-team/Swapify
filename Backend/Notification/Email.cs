using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Options;
using Mustache;

namespace FRITeam.Swapify.Backend.Notification
{
    public class Email
    {
        public string FromEmail { get; set; } = "Mailgun Sandbox <postmaster @sandbox260b1e74a61b4b7faf43e42dd656e3a0.mailgun.org>";
        public string ToEmail { get; set; } = "ExampleName<youremail@gmail.com>";
        public string Body { get; set; }
        public string Subject { get; set; }

        public Email()
        {

        }

        public void AddAttachmentToTemplate(MailMessage message)
        {
            using (message)
            {
                Attachment mac = new Attachment(@"../../../EmailTemplate/images/home -background.png");
                message.Attachments.Add(mac);
                string contentID = "Mac";
                mac.ContentId = contentID;

                Attachment swapify = new Attachment("../../../EmailTemplate/images/swapify.png");
                message.Attachments.Add(swapify);
                string contentID2 = "Swapify";
                swapify.ContentId = contentID2;
                
                swapify.ContentDisposition.Inline = true;
                swapify.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                mac.ContentDisposition.Inline = true;
                mac.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            }
        }

        public string GetRegistrationEmailSubject()
        {
            return Subject = "Registrácia na SWAPIFY";
        }

        public string GetRegistrationEmailBody(string baseUrl,string confirmLink)
        {
            using (var reader = new StreamReader("../../../EmailTemplate/RegistrationTemplate.html"))
            {
                Body = reader.ReadToEnd();
            }

            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(Body);
            Body = generator.Render(new
            {
                link = baseUrl,
                confirmationLink = confirmLink,
                img1 = "cid:mac",
                img2 = "cid:swapify"
            });
            return Body;
        }

        public string GetRestorePasswordEmailSubject()
        {
            return Subject = "Zabudnuté heslo";
        }

        public string GetRestorePasswordEmailBody(string baseUrl, string resetPasswordLink)
        {
            using (var reader = new StreamReader("../../../EmailTemplate/RestorePasswordTemplate.html"))
            {
                Body = reader.ReadToEnd();
            }

            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(Body);
            Body = generator.Render(new
            {
                link = baseUrl,
                restorePasswordLink = resetPasswordLink,
                img1 = "cid:mac",
                img2 = "cid:swapify"
            });
            return Body;
        }
    }
}
