using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRITeam.Swapify.APIWrapper
{
    public static class ResponseParser
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static ScheduleWeekContent ParseResponse(string myResponse)
        {

            var response = JObject.Parse(myResponse);
            // check if error occured
            var report = response["report"].ToString();
            if (!string.IsNullOrWhiteSpace(report))
            {
                var ex = new ArgumentException(report);
                _logger.Error(ex);
                throw ex;
            }

            var scheduleContent = (JArray)response["ScheduleContent"];

            var weekTimetable = ScheduleWeekContent.Build();

            foreach (var block in scheduleContent)
            {
                try
                {
                    {
                        LessonType lessonType = ConvertLessonType(block["tu"].ToString()[0]);
                        string teacherName = block["u"].ToString();
                        string roomName = block["r"].ToString();
                        string subjectShortcut = block["k"].ToString();
                        string subjectName = block["s"].ToString();
                        var sc = new ScheduleHourContent(int.Parse(block["b"].ToString()), false,
                                                         lessonType, teacherName, roomName, subjectShortcut,
                                                         subjectName, SubjectType.None, new List<string>());
                        //-1 because API count day 1,2,3,4,5 and we store days at indexes 0,1,2,3,4
                        weekTimetable.DaysInWeek[int.Parse(block["dw"].ToString()) - 1].BlocksInDay.Add(sc);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }
            return weekTimetable;
        }

        private static LessonType ConvertLessonType(char lessonShortcutType)
        {
            switch (lessonShortcutType)
            {
                case 'L': return LessonType.Laboratory;
                case 'P': return LessonType.Lecture;
                case 'C': return LessonType.Excercise;
                default: throw new ArgumentException($"Unexpected lesson type '{lessonShortcutType}'");
            }
        }
    }
}
