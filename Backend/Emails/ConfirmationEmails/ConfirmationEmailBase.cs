using FRITeam.Swapify.Backend.Exceptions;
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

        protected override void CreateEmailBody()
        {
            ReadTemplate();
            string logoImgContentId = AddImgToBodyBuilder(Path.GetFullPath(Path.Combine(OutputDirLocation, PathToLogo)));

            var compiler = new HtmlFormatCompiler();
            var generator = compiler.Compile(BodyBuilder.HtmlBody);
            BodyBuilder.HtmlBody = generator.Render(new
            {
                link = BaseUrl,
                confirmationLink = ConfirmationLink,
                img1 = logoImgContentId
            });
        }

        private void ReadTemplate()
        {
            string templatePath = Path.GetFullPath(Path.Combine(OutputDirLocation, PathToTemplate));
            if (!File.Exists(templatePath))
                throw new EmailException(GetType().Name, $"Email template '{templatePath}' does not exists.");
            try
            {
                BodyBuilder.HtmlBody = File.ReadAllText(templatePath);
            }
            catch (Exception e)
            {
                Logger.LogError($"Error when reading file '{templatePath}'. {e.Message}.");
                throw;
            }
            if (string.IsNullOrEmpty(BodyBuilder.HtmlBody))
                throw new EmailException(GetType().Name, $"Email template '{templatePath}' is null or empty.");
        }
    }
}
