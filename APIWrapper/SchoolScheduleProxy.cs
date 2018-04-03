using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolScheduleProxy : ISchoolScheduleProxy
    {
        private const string URL = "https://nic.uniza.sk/webservices";
        private const string SCHEDULE_CONTENT_URL = "getUnizaScheduleContent.php";
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ScheduleWeekContent GetByTeacherName(string teacherNumber)
        {
            return CallScheduleContentApi(1, teacherNumber);
        }

        public ScheduleWeekContent GetByStudyGroup(string studyGroupNumber)
        {
            return CallScheduleContentApi(2, studyGroupNumber);
        }

        public ScheduleWeekContent GetByRoomNumber(string roomNumber)
        {
            return CallScheduleContentApi(3, roomNumber);
        }

        public ScheduleWeekContent GetBySubjectCode(string subjectCode)
        {
            return CallScheduleContentApi(4, subjectCode);
        }

        private ScheduleWeekContent CallScheduleContentApi(int type, string requestContent)
        {
            var address = $"{URL}/{SCHEDULE_CONTENT_URL}?m={type}&id={Uri.EscapeUriString(requestContent)}";
            var myResponse = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";

                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
            return ParseResponse(myResponse);
        }

        private ScheduleWeekContent ParseResponse(string myResponse)
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
                            LessonType lessonType = this.ConvertLessonType(b["t"].ToString()[0]);
                            string teacherName = b["u"].ToString();
                            string roomName = b["r"].ToString();
                            string subjectShortcut = b["s"].ToString();
                            string subjectName = b["k"].ToString();
                            List<string> studyGroups = b["g"].ToString().Split(',').Select(x => x.Trim()).ToList();
                            SubjectType subjectType = (SubjectType)Convert.ToInt32(b["p"].ToString());
                            sc = new ScheduleHourContent(blockNumber++, isBlocked, lessonType, teacherName, roomName, subjectShortcut, subjectName, subjectType,studyGroups);
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

        private LessonType ConvertLessonType(char lessonShortcutType)
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
