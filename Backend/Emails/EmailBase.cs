using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Utils;
using System.IO;
using System.Reflection;

namespace FRITeam.Swapify.Backend.Emails
{
    public abstract class EmailBase
    {
        protected abstract string PathToTemplate { get; }
        protected abstract string Subject { get; }

        protected ILogger Logger { get; }
        protected string OutputDirLocation { get; }
        protected MailboxAddress Sender { get; set; }
        protected MailboxAddress Receiver { get; set; }
        protected string BaseUrl { get; set; }
        protected BodyBuilder BodyBuilder { get; set; }

        protected EmailBase(ILoggerFactory loggerFactory, string sender, string senderDisplayName, string receiver, string baseUrl)
        {
            Logger = loggerFactory.CreateLogger(GetType().FullName);
            OutputDirLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Sender = new MailboxAddress(senderDisplayName, sender);
            Receiver = new MailboxAddress(receiver);
            BaseUrl = baseUrl;
            BodyBuilder = new BodyBuilder();
        }

        public abstract MimeMessage CreateMailMessage();
        protected abstract bool CreateEmailBody();

        protected string AddImgToBodyBuilder(string imgPath)
        {
            if (!File.Exists(imgPath))
            {
                Logger.LogWarning($"Unable to load img {imgPath}.");
                return string.Empty;
            }
            MimeEntity entity = BodyBuilder.LinkedResources.Add(imgPath);
            entity.ContentId = MimeUtils.GenerateMessageId();
            return entity.ContentId;
        }
    }
}
