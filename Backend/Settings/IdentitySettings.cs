using System.Reflection;

namespace FRITeam.Swapify.Backend.Settings
{
    public class IdentitySettings : SettingsBase
    {
        public bool? RequireDigit { get; set; }
        public int? RequiredLength { get; set; }
        public bool? RequireNonAlphanumeric { get; set; }
        public bool? RequireUppercase { get; set; }
        public bool? RequireLowercase { get; set; }
        public bool? RequireConfirmedEmail { get; set; }
        public int? DefaultLockoutTimeSpan { get; set; }
        public int? MaxFailedAccessAttempts { get; set; }
        public bool? RequireUniqueEmail { get; set; }

        public IdentitySettings()
        {

        }

        public override void Validate()
        {
            foreach (PropertyInfo info in typeof(IdentitySettings).GetProperties())
            {
                if (info.GetValue(this, null) == null)
                    Errors.AppendLine($"Setting {info.Name} is missing in {nameof(IdentitySettings)} configuration section.");
            }

            CheckErrors("appsettings.json");
        }
    }
}
