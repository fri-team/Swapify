using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using FRITeam.Swapify.Backend.Enums;
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

        public void ComposeAndSendMail(EmailTypes typeOfEmail, EmailBase email, string generatedLink)
        {
            if (typeOfEmail == EmailTypes.RegistrationEmail)
            {
                RegistrationEmail regMail = new RegistrationEmail(email);
                regMail.GetSubject();
                regMail.GetBody(_urlSettings.DevelopUrl, generatedLink);
                ComposeMessage(regMail);
            }
            else if (typeOfEmail == EmailTypes.RestorePaswordEmail)
            {
                RestorePasswordEmail passwordEmail = new RestorePasswordEmail(email);
                passwordEmail.GetSubject();
                passwordEmail.GetBody(_urlSettings.DevelopUrl, generatedLink);
                ComposeMessage(passwordEmail);
            }
        }

        public void ComposeMessage(EmailBase data)
        {
            using (MailMessage message = new MailMessage(data.FromEmail, data.ToEmail))
            {
                message.Subject = data.Subject;
                message.Body = data.Body;
                data.AddAttachmentToTemplate(message);
                message.IsBodyHtml = true;

                SendMailMessage(message);
            }
        }

        public void SendMailMessage(MailMessage message)
        {
            NetworkCredential credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, (int)_emailSettings.SmtpPort))
            {
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Send(message);
            }
        }
    }
}
