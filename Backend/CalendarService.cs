using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRITeam.Swapify.Backend
{
    public class CalendarService : ICalendarService
    {
        private readonly CalendarSettings _calendarSettings;

        public CalendarService(IOptions<CalendarSettings> calendarSettings)
        {
            _calendarSettings = calendarSettings.Value;
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
            DateTime now = DateTime.Today;

            DateTime start;

            if (((int)now.DayOfWeek) > ((int)block.Day))
            {
                start = now.AddDays(7).AddDays(-(((int)now.DayOfWeek) - ((int)block.Day)))
                    .AddHours(block.StartHour);
            } else
            {
                start = now.AddDays(((int)block.Day) - ((int)now.DayOfWeek))
                    .AddHours(block.StartHour);
            }
            

            //with time zone specified
            return stringBuilder
                .AppendLine("BEGIN:VEVENT")
                .AppendLine("DTSTART;TZID=Europe/Amsterdam:" + start.ToString("yyyyMMddTHHmm00"))
                .AppendLine("DTEND;TZID=Europe/Amsterdam:" + start.AddHours(block.Duration).ToString("yyyyMMddTHHmm00"))
                .AppendLine("RRULE:FREQ=WEEKLY;UNTIL=" + EndOfSemester().ToString("yyyyMMddT000000"))
                .AppendLine("SUMMARY:" + course.CourseName)
                .AppendLine("DESCRIPTION:" + block.Room + ", " + block.Teacher)
                .AppendLine("PRIORITY:3")
                .AppendLine("END:VEVENT");
        }

        public DateTime EndOfSemester()
        {
            DateTime dateNow = DateTime.Now;
            if (dateNow.CompareTo(_calendarSettings.StartWinter) == 1 && dateNow.CompareTo(_calendarSettings.EndWinter) == -1)
            {
                return _calendarSettings.EndWinter;
            }
            else
            {
                return _calendarSettings.EndSummer;
            }
        }

        public StringBuilder EndCalendar(StringBuilder stringBuilder)
        {
            return stringBuilder.AppendLine("END:VCALENDAR");
        }
    }
}
