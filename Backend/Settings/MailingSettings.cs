using System.Text.RegularExpressions;

namespace FRITeam.Swapify.Backend.Settings
{
    public class MailingSettings : SettingsBase
    {
        public string EmailsNameSpace { get; set; }
        public string SmtpServer { get; set; }
        public int? SmtpPort { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(EmailsNameSpace))
                Errors.AppendLine($"Setting {nameof(EmailsNameSpace)} is missing in {nameof(MailingSettings)} configuration section.");
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

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if(!regex.IsMatch(Username))
                Errors.AppendLine($"{Username} is not valid email address.");

            CheckErrors("appsettings.json");
        }
    }
}
