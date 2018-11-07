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

        public void SendRegistrationConfirmationEmail(string receiverEmail, string confirmationLink)
        {
            NetworkCredential credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, (int)_emailSettings.SmtpPort))
            {
                client.Credentials = credentials;
                client.EnableSsl = true;

                string body = $"Pre potvrdenie účtu klikni na tento <a href='{confirmationLink}'>odkaz</a>.";
                MailMessage message = new MailMessage(_emailSettings.Username, receiverEmail, "Swapify - potvrdenie registrácie", body);
                message.IsBodyHtml = true;
                client.Send(message);
            }
        }

        public void SendResetPasswordEmail(string receiverEmail, string resetLink)
        {
            NetworkCredential credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, (int)_emailSettings.SmtpPort))
            {
                client.Credentials = credentials;
                client.EnableSsl = true;

                string body = $"Pre reset hesla klikni na tento <a href='{resetLink}'>odkaz</a>.";
                MailMessage message = new MailMessage(_emailSettings.Username, receiverEmail, "Swapify - obnovenie hesla", body);
                message.IsBodyHtml = true;
                client.Send(message);
            }
        }
    }
}
