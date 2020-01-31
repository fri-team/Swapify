using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using Newtonsoft.Json;
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

        public static IEnumerable<ScheduleHourContent> ParseResponse(string myResponse)
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

            var weekTimetable = new List<ScheduleHourContent>();

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
                        var hourContent = new ScheduleHourContent(int.Parse(block["dw"].ToString()) - 1, int.Parse(block["b"].ToString()), false,
                                                         lessonType, teacherName, roomName,
                                                         subjectShortcut, subjectName, SubjectType.None);

                        weekTimetable.Add(hourContent);                        
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

        public static IEnumerable<CourseContent> ParseCourseResponse(string myResponse)
        {
            var response = JObject.Parse(myResponse);
            var coursesContent = (JObject)response["ScheduleTypeList"];
            var childs = coursesContent.Children().ToList();
            childs.RemoveAt(0);

            var courses = new List<CourseContent>();
            foreach (var child in childs)
            {
                try
                {
                    {
                        CourseContent courseContent = child.First.ToObject<CourseContent>();
                        courses.Add(courseContent);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }

            return courses;
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
