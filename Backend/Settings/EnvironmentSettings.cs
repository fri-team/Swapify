
namespace FRITeam.Swapify.Backend.Settings
{
    public class EnvironmentSettings : SettingsBase
    {
        public string Environment { get; set; }
        public string JwtSecret { get; set; }

        public EnvironmentSettings()
        {

        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Environment))
                _errors.AppendLine($"Setting {nameof(Environment)} is missing in environmentVariables.");
            if (string.IsNullOrEmpty(JwtSecret))
                _errors.AppendLine($"Setting {nameof(JwtSecret)} is missing in environmentVariables.");

            CheckErrors("environmentVariables");
        }
    }
}
