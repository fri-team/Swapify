using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace FRITeam.Swapify.Backend
{
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly MailingSettings _emailSettings;
        private readonly EnvironmentSettings _environmentSettings;
        private readonly RecaptchaSettings _recaptchaSettings;

        public EmailService(ILoggerFactory loggerFactory, IOptions<MailingSettings> emailSettings,
            IOptions<EnvironmentSettings> environmentSettings, IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _logger = loggerFactory.CreateLogger(GetType().FullName);
            _loggerFactory = loggerFactory;
            _emailSettings = emailSettings.Value;
            _environmentSettings = environmentSettings.Value;
            _recaptchaSettings = recaptchaSettings.Value;
        }

        public String GetCaptchaInfo(int index)
        {
            if (index == 1)
            {
                return _recaptchaSettings.PrivateKey;
            }
            return _recaptchaSettings.URL;
        }

        public bool SendFeedbackEmail(string userEmail, string userName, string subject, string body)
        {
            try
            {
                MimeMessage mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(userName, userEmail));
                mailMessage.To.Add(new MailboxAddress(_emailSettings.FeedbackEmail));
                mailMessage.Subject = subject + " (" + userEmail + ")";
                mailMessage.Body = new TextPart("html")
                {
                    Text = body
                };

                SendEmail(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        public bool SendConfirmationEmail(string receiver, string confirmationLink, string emailType)
        {
            try
            {
                string type = $"{_emailSettings.EmailsNameSpace}.{emailType}";
                var email = Activator.CreateInstance(Type.GetType(type), _loggerFactory, _emailSettings.SenderEmail,
                    _emailSettings.SenderDisplayName, receiver, _environmentSettings.BaseUrl, confirmationLink);
                MimeMessage mailMessage = (MimeMessage)email.GetType().GetMethod("CreateMailMessage")
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

        private void SendEmail(MimeMessage message)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(_emailSettings.SmtpServer, (int)_emailSettings.SmtpPort, true);
                client.Authenticate(_emailSettings.Username, _emailSettings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
