using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace FRITeam.Swapify.Backend
{
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private readonly MailingSettings _emailSettings;
        private readonly EnvironmentSettings _environmentSettings;

        public EmailService(ILoggerFactory loggerFactory, IOptions<MailingSettings> emailSettings,
            IOptions<EnvironmentSettings> environmentSettings)
        {
            _logger = loggerFactory.CreateLogger(GetType().FullName);
            _emailSettings = emailSettings.Value;
            _environmentSettings = environmentSettings.Value;
        }

        public bool SendConfirmationEmail(string receiver, string confirmationLink, string emailType)
        {
            try
            {
                string type = $"FRITeam.Swapify.Backend.Emails.ConfirmationEmails.{emailType}";
                var email = Activator.CreateInstance(Type.GetType(type), _emailSettings.Username, _emailSettings.DisplayName, receiver);
                email.GetType().GetMethod("CreateConfirmationEmailBody")
                               .Invoke(email, new object[] { _environmentSettings.BaseUrl, confirmationLink });
                MailMessage mailMessage = (MailMessage)email.GetType().GetMethod("CreateMailMessage")
                                                                      .Invoke(email, null);
                SendEmail(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        private void SendEmail(MailMessage message)
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
