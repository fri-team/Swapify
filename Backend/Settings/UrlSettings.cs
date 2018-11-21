
namespace FRITeam.Swapify.Backend.Settings
{
    public class UrlSettings : SettingsBase
    {
        public string DomainUrl { get; set; }
        public string DevelopUrl { get; set; }

        public UrlSettings()
        {

        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(DomainUrl))
                Errors.AppendLine($"Setting {nameof(DomainUrl)} is missing in environmentVariables.");
            if (string.IsNullOrEmpty(DevelopUrl))
                Errors.AppendLine($"Setting {nameof(DevelopUrl)} is missing in environmentVariables.");

            CheckErrors("launchSettings.json");
        }
    }
}
