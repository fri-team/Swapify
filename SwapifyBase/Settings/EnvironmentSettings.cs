using System;

namespace FRITeam.Swapify.SwapifyBase.Settings
{
    public class EnvironmentSettings : SettingsBase
    {
        public override string ConfigFileName => "environmentVariables";
        public string Environment { get; set; }
        public string JwtSecret { get; set; }
        public string BaseUrl { get; set; }

        protected override void ValidateInternal()
        {
            if (!(Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
                Errors.AppendLine($"Setting {nameof(BaseUrl)} is not valid URL.");
        }
    }
}
