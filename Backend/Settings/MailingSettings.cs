
using System;
using System.Net.Mail;

namespace FRITeam.Swapify.Backend.Settings
{
    public class MailingSettings : SettingsBase
    {
        public string SmtpServer { get; set; }
        public int? SmtpPort { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        public MailingSettings()
        {

        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(SmtpServer))
                Errors.AppendLine($"Setting {nameof(SmtpServer)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(Username))
                Errors.AppendLine($"Setting {nameof(Username)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(DisplayName))
                Errors.AppendLine($"Setting {nameof(DisplayName)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(Password))
                Errors.AppendLine($"Setting {nameof(Password)} is missing in {nameof(MailingSettings)} configuration section.");
            if (SmtpPort == null)
                Errors.AppendLine($"Setting {nameof(SmtpPort)} is missing in {nameof(MailingSettings)} configuration section.");

            try
            {
#pragma warning disable S1481 // Unused local variables should be removed
                MailAddress m = new MailAddress(Username);
#pragma warning restore S1481 // Unused local variables should be removed
            }
            catch (FormatException)
            {
                Errors.AppendLine($"{Username} is not valid email address.");
            }

            CheckErrors("appsettings.json");
        }
    }
}
