using System.Collections.Generic;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleWeekContent
    {
        /// <summary>
        /// List of all days in week with schedule
        /// </summary>
        public readonly List<ScheduleDayContent> DaysInWeek = new List<ScheduleDayContent>();
    }
}
