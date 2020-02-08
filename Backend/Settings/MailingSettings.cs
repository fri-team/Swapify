using System.Text.RegularExpressions;

namespace FRITeam.Swapify.Backend.Settings
{
    public class MailingSettings : SettingsBase
    {
        public string EmailsNameSpace { get; set; }
        public string SmtpServer { get; set; }
        public int? SmtpPort { get; set; }
        public string Username { get; set; }
        public string SenderEmail { get; set; }
        public string SenderDisplayName { get; set; }
        public string Password { get; set; }
        public string FeedbackEmail { get; set; }

        protected override void ValidateInternal()
        {
            Regex regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (!regex.IsMatch(SenderEmail))
                Errors.AppendLine($"{SenderEmail} is not valid email address.");
        }
    }
}
