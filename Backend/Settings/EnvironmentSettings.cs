using System;

namespace FRITeam.Swapify.Backend.Settings
{
    public class EnvironmentSettings : SettingsBase
    {
        public override string ConfigFileName => "environmentVariables";
        public string Environment { get; set; }
        public string JwtSecret { get; set; }
        public string BaseUrl { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Environment))
                Errors.AppendLine($"Setting {nameof(Environment)} is missing in {ConfigFileName}.");
            if (string.IsNullOrEmpty(JwtSecret))
                Errors.AppendLine($"Setting {nameof(JwtSecret)} is missing in {ConfigFileName}.");
            if (string.IsNullOrEmpty(BaseUrl))
                Errors.AppendLine($"Setting {nameof(BaseUrl)} is missing in {ConfigFileName}.");
            if (!(Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
                Errors.AppendLine($"Setting {nameof(BaseUrl)} is not valid URL.");
        }
    }
}
