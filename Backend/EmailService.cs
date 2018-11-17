using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using FRITeam.Swapify.Backend.Notification;

namespace FRITeam.Swapify.Backend
{
    public class EmailService : IEmailService
    {
        private readonly MailingSettings _emailSettings;
        private readonly UrlSettings _urlSettings;

        public EmailService()
        {
        }

        public EmailService(IOptions<MailingSettings> emailSettings, IOptions<UrlSettings> urlSettings)
        {
            _emailSettings = emailSettings.Value;
            _urlSettings = urlSettings.Value;
        }

        public virtual void SendReqistrationMail(Email data, string confirmationLink)
        {
            using (MailMessage message = new MailMessage(data.FromEmail, data.ToEmail))
            {
                message.Subject = data.GetRegistrationEmailSubject();
                message.Body = data.GetRegistrationEmailBody(_urlSettings.ApplicationUrl ,confirmationLink);
                data.AddAttachmentToTemplate(message);
                message.IsBodyHtml = true;

                SendMailMessage(message);
            }
        }

        public virtual void SendRestorePaswordMail(Email data, string resetPasswordLink)
        {
            using (MailMessage message = new MailMessage(data.FromEmail, data.ToEmail))
            {
                message.Subject = data.GetRestorePasswordEmailSubject();
                message.Body = data.GetRestorePasswordEmailBody(_urlSettings.ApplicationUrl, resetPasswordLink);
                data.AddAttachmentToTemplate(message);
                message.IsBodyHtml = true;

                SendMailMessage(message);
            }
        }


        public virtual void SendMailMessage(MailMessage message)
        {
            NetworkCredential credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, (int) _emailSettings.SmtpPort))
            {
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Send(message);
            }
        }
    }
}
