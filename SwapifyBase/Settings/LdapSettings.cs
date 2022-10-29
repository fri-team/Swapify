namespace FRITeam.Swapify.SwapifyBase.Settings
{
    public class LdapSettings : SettingsBase
    {

        public string AlternativMailPrefix { get; set; }
        public string MailPrefix { get; set; }
        public string SecureSocketLayer { get; set; }
        public string BaseDN { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
    }
}
