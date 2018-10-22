using FRITeam.Swapify.Backend.Exceptions;

namespace FRITeam.Swapify.Backend.Settings
{
    public class MailingSettings : IValidatable
    {
        public string SmtpServer { get; set; }
        public int? SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public MailingSettings()
        {

        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(SmtpServer))
                throw new SettingException("appsettings.json", $"Setting {nameof(SmtpServer)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(Username))
                throw new SettingException("appsettings.json", $"Setting {nameof(Username)} is missing in {nameof(MailingSettings)} configuration section.");
            if (string.IsNullOrEmpty(Password))
                throw new SettingException("appsettings.json", $"Setting {nameof(Password)} is missing in {nameof(MailingSettings)} configuration section.");
            if (SmtpPort == null)
                throw new SettingException("appsettings.json", $"Setting {nameof(SmtpPort)} is missing in {nameof(MailingSettings)} configuration section.");
        }
    }
}
