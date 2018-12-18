namespace FRITeam.Swapify.Backend.Settings
{
    public class PathSettings : SettingsBase
    {
        public string CoursesJsonPath { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(CoursesJsonPath))
                Errors.AppendLine($"Setting {nameof(CoursesJsonPath)} is missing in {nameof(PathSettings)} configuration section.");
        }
    }
}
