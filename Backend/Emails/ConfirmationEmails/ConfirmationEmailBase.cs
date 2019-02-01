using Microsoft.Extensions.Logging;
using MimeKit;
using Mustache;
using System;
using System.IO;

namespace FRITeam.Swapify.Backend.Emails
{
    public abstract class ConfirmationEmailBase : EmailBase
    {
        protected const string PathToLogo = @"Emails/EmailTemplates/images/swapify_logo.png";
        protected string ConfirmationLink { get; set; }

        protected ConfirmationEmailBase(ILoggerFactory loggerFactory, string sender, string senderDisplayName, string receiver, string baseUrl, string confirmationLink)
            : base(loggerFactory, sender, senderDisplayName, receiver, baseUrl)
        {
            ConfirmationLink = confirmationLink;
        }

        public override MimeMessage CreateMailMessage()
        {
            if (!CreateEmailBody())
            {
                Logger.LogError($"Unable to create email body.");
                return null;
            }

            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(Sender);
            mailMessage.To.Add(Receiver);
            mailMessage.Subject = Subject;
            mailMessage.Body = BodyBuilder.ToMessageBody();
            return mailMessage;
        }

        protected override bool CreateEmailBody()
        {
            BodyBuilder.HtmlBody = ReadTemplate();
            if (string.IsNullOrEmpty(BodyBuilder.HtmlBody))
                return false;

            string logoImgContentId = AddImgToBodyBuilder(Path.GetFullPath(Path.Combine(OutputDirLocation, PathToLogo)));

            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(BodyBuilder.HtmlBody);
            BodyBuilder.HtmlBody = generator.Render(new
            {
                link = BaseUrl,
                confirmationLink = ConfirmationLink,
                img1 = logoImgContentId
            });
            return true;
        }

        private string ReadTemplate()
        {
            string templatePath = Path.Combine(OutputDirLocation, PathToTemplate);
            if (!File.Exists(templatePath))
            {
                Logger.LogError($"Email template {templatePath} does not exists.");
                return null;
            }
            try
            {
                return File.ReadAllText(templatePath);
            }
            catch (Exception e)
            {
                Logger.LogError($"Error when reading file {templatePath}.\n {e.ToString()}.");
                return null;
            }
        }
    }
}
