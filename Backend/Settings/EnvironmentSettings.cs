
namespace FRITeam.Swapify.Backend.Settings
{
    public class EnvironmentSettings : SettingsBase
    {
        public string Environment { get; set; }
        public string JwtSecret { get; set; }
        public string BaseUrl { get; set; }

        public EnvironmentSettings()
        {
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Environment))
                Errors.AppendLine($"Setting {nameof(Environment)} is missing in environmentVariables.");
            if (string.IsNullOrEmpty(JwtSecret))
                Errors.AppendLine($"Setting {nameof(JwtSecret)} is missing in environmentVariables.");
            if (string.IsNullOrEmpty(BaseUrl))
                Errors.AppendLine($"Setting {nameof(BaseUrl)} is missing in environmentVariables.");

            CheckErrors("environmentVariables");
        }
    }
}
