
namespace FRITeam.Swapify.Backend.Settings
{
    public class MailingSettings : SettingsBase
    {
        public string SmtpServer { get; set; }
        public int? SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public MailingSettings()
        {

        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(SmtpServer))
                _errors.AppendLine($"Setting {nameof(SmtpServer)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(Username))
                _errors.AppendLine($"Setting {nameof(Username)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(Password))
                _errors.AppendLine($"Setting {nameof(Password)} is missing in {nameof(MailingSettings)} configuration section.");
            if (SmtpPort == null)
                _errors.AppendLine($"Setting {nameof(SmtpPort)} is missing in {nameof(MailingSettings)} configuration section.");

            CheckErrors("appsettings.json");
        }
    }
}
