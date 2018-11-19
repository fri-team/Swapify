using FRITeam.Swapify.Backend.Email;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FRITeam.Swapify.Backend
{
    public class EmailService : IEmailService
    {
        private readonly MailingSettings _emailSettings;

        public EmailService()
        {
        }

        public EmailService(IOptions<MailingSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(Email.Email email)
        {
            NetworkCredential credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, (int)_emailSettings.SmtpPort))
            {
                client.Credentials = credentials;
                client.EnableSsl = true;                
                MailMessage message = new MailMessage(_emailSettings.Username, email.ToEmail, email.Subject, email.Body);
                message.IsBodyHtml = true;
                client.Send(message);
            }
        }

        //public virtual void SendRegistrationConfirmationEmail(string receiverEmail, string confirmationLink)
        //{
        //    NetworkCredential credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
        //    using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, (int)_emailSettings.SmtpPort))
        //    {
        //        client.Credentials = credentials;
        //        client.EnableSsl = true;

        //        string body = $"Pre potvrdenie účtu klikni na tento <a href='{confirmationLink}'>odkaz</a>.";
        //        MailMessage message = new MailMessage(_emailSettings.Username, receiverEmail, "Swapify - potvrdenie registrácie", body);
        //        message.IsBodyHtml = true;
        //        client.Send(message);
        //    }
        //}
    }
}
