using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FRITeam.Swapify.APIWrapper
{
    public static class ResponseParser
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static List<ScheduleContent> ParseResponse(string myResponse)
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

            var weekTimetable = new List<ScheduleContent>();

            foreach (var block in scheduleContent)
            {
                try
                {
                    {
                        LessonType lessonType = ConvertLessonType(block["tu"].ToString()[0]);
                        string teacherName = block["u"].ToString();
                        string roomName = block["r"].ToString();
                        string subjectCode = block["k"].ToString().Trim();
                        string subjectShortcut = block["p"].ToString().Trim();
                        string subjectNameHelper = block["s"].ToString().Trim();                        
                        string subjectName = subjectNameHelper.First().ToString().ToUpper() + subjectNameHelper.Substring(1);                        
                        var content = new ScheduleContent()
                        {
                            BlockNumber = int.Parse(block["dw"].ToString()) - 1,
                            LessonType = lessonType,
                            TeacherName = teacherName,
                            RoomName = roomName,
                            CourseCode = subjectCode,
                            CourseShortcut = subjectShortcut,
                            CourseName = subjectName                            
                        };
                        weekTimetable.Add(content); 
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
