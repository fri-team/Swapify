using System;
using System.Collections.Generic;
using System.Text;

namespace APIWrapper.Objects
{
    public class ScheduleWeekContent
    {
        /// <summary>
        ///         List of all days in week with schedule
        /// </summary>
        public List<ScheduleDayContent> DaysInWeek;

        public ScheduleWeekContent()
        {
            DaysInWeek = new List<ScheduleDayContent>();
        }
    }
}
