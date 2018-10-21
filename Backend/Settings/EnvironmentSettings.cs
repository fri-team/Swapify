using System;

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
            if (string.IsNullOrEmpty(Environment))
                throw new ArgumentException($"Setting {nameof(Environment)} is missing in launchSettings.json environmentVariables configuration section.");
            if (string.IsNullOrEmpty(JwtSecret))
                throw new ArgumentException($"Setting {nameof(JwtSecret)} is missing in launchSettings.json environmentVariables configuration section.");

        }
    }
}
