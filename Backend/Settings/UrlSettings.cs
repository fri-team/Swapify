
namespace FRITeam.Swapify.Backend.Settings
{
    public class UrlSettings : SettingsBase
    {
        public string ApplicationUrl { get; set; }

        public UrlSettings()
        {

        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(ApplicationUrl))
            {
                Errors.AppendLine($"Setting {nameof(ApplicationUrl)} is missing in {nameof(UrlSettings)} configuration section.");
            }
            CheckErrors("appsettings.json");
        }
    }
}
