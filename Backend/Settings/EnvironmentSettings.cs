using FRITeam.Swapify.Backend.Exceptions;

namespace FRITeam.Swapify.Backend.Settings
{
    public class EnvironmentSettings : IValidatable
    {
        public string Environment { get; set; }
        public string JwtSecret { get; set; }

        public EnvironmentSettings()
        {

        }

        public void Validate()
        {
            //  See issue #87
            //if (string.IsNullOrEmpty(Environment))
            //    throw new SettingException("launchSettings.json", $"Setting {nameof(Environment)} is missing in environmentVariables configuration section.");
            //if (string.IsNullOrEmpty(JwtSecret))
            //    throw new SettingException("launchSettings.json", $"Setting {nameof(JwtSecret)} is missing in environmentVariables configuration section.");
        }
    }
}
