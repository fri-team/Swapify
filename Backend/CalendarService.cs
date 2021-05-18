using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Text;

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
            bool isWinter = dateNow.Month >= _calendarSettings.StartWinterMonth && dateNow.Month <= _calendarSettings.EndWinterMonth && dateNow.Day <= _calendarSettings.EndWinterDay;

            if (isWinter)
            {
                startDateTime = new DateTime(dateNow.Year, _calendarSettings.StartWinterMonth, _calendarSettings.StartWinterDay);
                endDateTime = new DateTime(dateNow.Year, _calendarSettings.EndWinterMonth, _calendarSettings.EndWinterDay).AddDays(1); // we add one day for recurring events
            }
            else
            {
                bool isNextYear = dateNow.Month != 12;
                startDateTime = new DateTime(dateNow.Year + (isNextYear ? 0 : 1), _calendarSettings.StartSummerMonth, _calendarSettings.StartSummerDay);
                endDateTime = new DateTime(dateNow.Year + (isNextYear ? 0 : 1), _calendarSettings.EndSummerMonth, _calendarSettings.EndSummerDay).AddDays(1); // we add one day for recurring events
            }
            dayOfWeek = dateNow.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) dateNow.DayOfWeek;
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
            // do not create past events - start today
            if(dayOfWeek > (int) block.Day)
            {
                return stringBuilder;
            }

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
