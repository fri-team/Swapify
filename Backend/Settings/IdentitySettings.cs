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
    }
}
