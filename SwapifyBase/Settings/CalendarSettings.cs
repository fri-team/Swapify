using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.SwapifyBase.Settings
{
    public class CalendarSettings : SettingsBase
    {
        public DateTime StartWinter { get; set; }
        public DateTime EndWinter { get; set; }
        public DateTime StartSummer { get; set; }
        public DateTime EndSummer { get; set; }
    }
}
