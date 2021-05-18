namespace FRITeam.Swapify.Backend.Settings
{
    public class CalendarSettings : SettingsBase
    {
        public int StartWinterDay { get; set; }
        public int StartWinterMonth { get; set; }
        public int EndWinterDay { get; set; }
        public int EndWinterMonth { get; set; }
        public int StartSummerDay { get; set; }
        public int StartSummerMonth { get; set; }
        public int EndSummerDay { get; set; }
        public int EndSummerMonth { get; set; }
    }
}
