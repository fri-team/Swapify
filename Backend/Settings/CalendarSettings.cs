using System;

namespace FRITeam.Swapify.Backend.Settings
{
    public class CalendarSettings : SettingsBase
    {
        public DateTime StartWinter { get; set; }
        public DateTime EndWinter { get; set; }
        public DateTime StartSummer { get; set; }
        public DateTime EndSummer { get; set; }
    }
}
