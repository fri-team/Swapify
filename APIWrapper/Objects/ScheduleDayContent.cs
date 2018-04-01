using System.Collections.Generic;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleDayContent
    {
        /// <summary>
        /// Each day has 13 block - each hour 1 block
        /// </summary>
        public readonly List<ScheduleHourContent> BlocksInDay = new List<ScheduleHourContent>();
    }
}
