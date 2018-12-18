using System.Collections.Generic;

namespace FRITeam.Swapify.APIWrapper.Objects
{
    public class ScheduleWeekContent
    {
        /// <summary>
        /// List of all days in week with schedule
        /// </summary>
        public readonly List<ScheduleDayContent> DaysInWeek = new List<ScheduleDayContent>();

        public static ScheduleWeekContent Build()
        {
            var weekTimetable = new ScheduleWeekContent();
            for (int i = 0; i < 5; i++)
            {
                weekTimetable.DaysInWeek.Add(new ScheduleDayContent());
            }
            return weekTimetable;
        }
    }
}
