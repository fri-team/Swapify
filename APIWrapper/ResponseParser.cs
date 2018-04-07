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
    public class ResponseParser
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
            var parsedResponse = new ScheduleWeekContent();
            foreach (var blocks in scheduleContent)
            {
                var blockNumber = 1;
                var daySchedule = new ScheduleDayContent();
                foreach (var b in blocks)
                {
                    ScheduleHourContent sc = null;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(b["t"].ToString()) && !string.IsNullOrWhiteSpace(b["p"].ToString()))
                        {
                            bool isBlocked = Convert.ToBoolean(int.Parse(b["b"].ToString()));
                            LessonType lessonType = ConvertLessonType(b["t"].ToString()[0]);
                            string teacherName = b["u"].ToString();
                            string roomName = b["r"].ToString();
                            string subjectShortcut = b["s"].ToString();
                            string subjectName = b["k"].ToString();
                            List<string> studyGroups = b["g"].ToString().Split(',').Select(x => x.Trim()).ToList();
                            SubjectType subjectType = (SubjectType)Convert.ToInt32(b["p"].ToString());
                            sc = new ScheduleHourContent(blockNumber++, isBlocked, lessonType, teacherName, roomName, subjectShortcut, subjectName, subjectType, studyGroups);
                        }

                        daySchedule.BlocksInDay.Add(sc);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        throw ex;
                    }

                }
                parsedResponse.DaysInWeek.Add(daySchedule);
            }
            return parsedResponse;
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
