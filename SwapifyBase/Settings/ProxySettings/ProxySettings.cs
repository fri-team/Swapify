using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.SwapifyBase.Settings.ProxySettings
{
    public class ProxySettings : SettingsBase
    {
        public string Base_URL { get; set; }
        public string ScheduleContentURL { get; set; }
        public string CourseContentURL { get; set; }
        public StudyYear StudyYear { get; set; }                  
    }
}
