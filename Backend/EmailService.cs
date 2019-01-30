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
        private readonly ILoggerFactory _loggerFactory;
        private readonly MailingSettings _emailSettings;
        private readonly EnvironmentSettings _environmentSettings;

        public EmailService(ILoggerFactory loggerFactory, IOptions<MailingSettings> emailSettings,
            IOptions<EnvironmentSettings> environmentSettings)
        {
            _logger = loggerFactory.CreateLogger(GetType().FullName);
            _loggerFactory = loggerFactory;
            _emailSettings = emailSettings.Value;
            _environmentSettings = environmentSettings.Value;
        }

        public bool SendConfirmationEmail(string receiver, string confirmationLink, string emailType)
        {
            try
            {
                string type = $"{_emailSettings.EmailsNameSpace}.{emailType}";
                var email = Activator.CreateInstance(Type.GetType(type), _loggerFactory, _emailSettings.SenderEmail,
                    _emailSettings.SenderDisplayName, receiver, _environmentSettings.BaseUrl, confirmationLink);
                MailMessage mailMessage = (MailMessage)email.GetType().GetMethod("CreateMailMessage")
                                                                      .Invoke(email, null);
                if (mailMessage == null)
                {
                    _logger.LogError($"Unable to create MailMessage for email type '{emailType}'.");
                    return false;
                }
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
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Send(message);
            }
        }
    }
}
