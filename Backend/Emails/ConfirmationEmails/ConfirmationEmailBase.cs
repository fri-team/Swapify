using Microsoft.Extensions.Logging;
using Mustache;
using System;
using System.IO;
using System.Net.Mail;

namespace FRITeam.Swapify.Backend.Emails
{
    public abstract class ConfirmationEmailBase : EmailBase
    {
        protected const string PathToMac = @"Emails/EmailTemplates/images/home-background.png";
        protected const string PathToLogo = @"Emails/EmailTemplates/images/swapify.png";
        protected const string MacContentId = "Mac";
        protected const string LogoContentId = "Swapify";
        protected const string MacImg = "cid:" + MacContentId;
        protected const string LogoImg = "cid:" + LogoContentId;
        protected string ConfirmationLink { get; set; }
        private readonly ILogger _logger;

        protected ConfirmationEmailBase(ILoggerFactory loggerFactory, string sender, string senderDisplayName, string receiver, string baseUrl, string confirmationLink)
            : base(sender, senderDisplayName, receiver, baseUrl)
        {
            ConfirmationLink = confirmationLink;
            _logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        public override MailMessage CreateMailMessage()
        {
            if (!CreateEmailBody())
            {
                _logger.LogError($"Unable to create email body.");
                return null;
            }

            MailMessage mailMessage = new MailMessage(Sender, Receiver);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.Sender = Sender;
            mailMessage.IsBodyHtml = true;

            string macImgPath = Path.Combine(OutputDirLocation, PathToMac);
            string logoImgPath = Path.Combine(OutputDirLocation, PathToLogo);
            if (ImgsExists(macImgPath, logoImgPath))
            {
                Attachment macImg = CreateImgAttachment(macImgPath, MacContentId);
                mailMessage.Attachments.Add(macImg);
                Attachment swapifyImg = CreateImgAttachment(logoImgPath, LogoContentId);
                mailMessage.Attachments.Add(swapifyImg);
            }
            return mailMessage;
        }

        protected override bool CreateEmailBody()
        {
            Body = ReadTemplate();
            if (Body == null)
                return false;

            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(Body);
            Body = generator.Render(new
            {
                link = BaseUrl,
                confirmationLink = ConfirmationLink,
                img1 = MacImg,
                img2 = LogoImg
            });
            return true;
        }

        private string ReadTemplate()
        {
            string templatePath = Path.Combine(OutputDirLocation, PathToTemplate);
            if (!File.Exists(templatePath))
            {
                _logger.LogError($"Email template {templatePath} does not exists.");
                return null;
            }
            try
            {
                return File.ReadAllText(templatePath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error when reading file {templatePath}.\n {e.ToString()}");
                return null;
            }
        }

        private bool ImgsExists(string macImgPath, string logoImgPath)
        {
            if (!File.Exists(macImgPath))
            {
                _logger.LogWarning("Unable to load macImg.");
                return false;
            }
            if (!File.Exists(logoImgPath))
            {
                _logger.LogWarning("Unable to load logoImg.");
                return false;
            }
            return true;
        }
    }
}
