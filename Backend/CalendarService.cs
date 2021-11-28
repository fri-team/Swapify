using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using FRITeam.Swapify.SwapifyBase.Settings;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace FRITeam.Swapify.Backend
{
    public class CalendarService : ICalendarService
    {
        private readonly int dayOfWeek = 0;
        private readonly CalendarSettings _calendarSettings;
        private readonly DateTime startDateTime;
        private readonly DateTime endDateTime;

        public CalendarService(IOptions<CalendarSettings> calendarSettings)
        {
            _calendarSettings = calendarSettings.Value;

            DateTime dateNow = DateTime.Now;
            if (dateNow.CompareTo(_calendarSettings.StartWinter) == 1 && dateNow.CompareTo(_calendarSettings.EndWinter) == -1)
            {
                startDateTime = _calendarSettings.StartWinter;
                endDateTime = _calendarSettings.EndWinter;
            }
            else
            {
                startDateTime = _calendarSettings.StartSummer;
                endDateTime = _calendarSettings.EndSummer;
            }
            dayOfWeek = dateNow.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)dateNow.DayOfWeek;
        }

        public StringBuilder StartCalendar()
        {
            return new StringBuilder()
                .AppendLine("BEGIN:VCALENDAR")
                .AppendLine("VERSION:2.0")
                .AppendLine("CALSCALE:GREGORIAN")
                .AppendLine("METHOD:PUBLISH")
                .AppendLine("BEGIN:VTIMEZONE")
                .AppendLine("TZID:Europe/Amsterdam")
                .AppendLine("BEGIN:STANDARD")
                .AppendLine("TZOFFSETTO:+0100")
                .AppendLine("TZOFFSETFROM:+0100")
                .AppendLine("END:STANDARD")
                .AppendLine("END:VTIMEZONE");
        }
        public StringBuilder CreateEvent(StringBuilder stringBuilder, Block block, Course course)
        {
            DateTime now = DateTime.Now;
            DateTime start = now.AddDays(-dayOfWeek + block.Day.GetHashCode())
                .AddHours(-now.Hour + block.StartHour)
                .AddMinutes(-now.Minute);

            //with time zone specified
            return stringBuilder
                .AppendLine("BEGIN:VEVENT")
                .AppendLine("DTSTART;TZID=Europe/Amsterdam:" + start.ToString("yyyyMMddTHHmm00"))
                .AppendLine("DTEND;TZID=Europe/Amsterdam:" + start.AddHours(block.Duration).ToString("yyyyMMddTHHmm00"))
                .AppendLine("RRULE:FREQ=WEEKLY;UNTIL=" + endDateTime.ToString("yyyyMMddT000000"))
                .AppendLine("SUMMARY:" + course.CourseName)
                .AppendLine("DESCRIPTION:" + block.Room + ", " + block.Teacher)
                .AppendLine("PRIORITY:3")
                .AppendLine("END:VEVENT");
        }

        public StringBuilder EndCalendar(StringBuilder stringBuilder)
        {
            return stringBuilder.AppendLine("END:VCALENDAR");
        }
    }
}
