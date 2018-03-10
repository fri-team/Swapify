using APIWrapper.Enums;
using APIWrapper.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace APIWrapper
{
    public class Proxy : IProxy
    {
        private const string __URL__ = "https://nic.uniza.sk/webservices";
        private const string __SCHEDULE_CONTENT_URL__ = "getUnizaScheduleContent.php";
        
        public ScheduleWeekContent GetScheduleContentByTeacherName(string teacherNumber)
        {
            return CallScheduleContentApi(1, teacherNumber);
        }

        public ScheduleWeekContent GetScheduleContentByStudyGroup(string studyGroupNumber)
        {
            return CallScheduleContentApi(2, studyGroupNumber);
        }

        public ScheduleWeekContent GetScheduleContentByRoomNumber(string roomNumber)
        {
            return CallScheduleContentApi(3, roomNumber);
        }

        public ScheduleWeekContent GetScheduleContentBySubjectCode(string subjectCode)
        {
            return CallScheduleContentApi(4, subjectCode);
        }

        private ScheduleWeekContent CallScheduleContentApi(int type, string requestContent)
        {
            string address = $"{__URL__}/{__SCHEDULE_CONTENT_URL__}?m={type}&id={Uri.EscapeUriString(requestContent)}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "Get";
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }
            
            return ParseResponse(myResponse);
        }

        private ScheduleWeekContent ParseResponse(string myResponse)
        {
            
            JObject joResponse = JObject.Parse(myResponse);
            //check if error occured
            string report = joResponse["report"].ToString();
            if (!String.IsNullOrWhiteSpace(report))
            {
                throw new ArgumentException(report);
            }
            
            JArray array = (JArray)joResponse["ScheduleContent"];
            ScheduleWeekContent ret = new ScheduleWeekContent();
            foreach (var element in array)
            {
                int blockNumber = 1;
                var daySchedule = new ScheduleDayContent();
                foreach (var blck in element)
                {
                    ScheduleHourContent sc = null;
                    try
                    {
                        sc = new ScheduleHourContent();
                        sc.BlockNumber = blockNumber++;
                        sc.IsBlocked = Convert.ToBoolean(int.Parse(blck["b"].ToString()));

                        if (!String.IsNullOrWhiteSpace(blck["t"].ToString()) && !String.IsNullOrWhiteSpace(blck["p"].ToString()))
                        {
                            sc.LessonType = this.ConvertLessonType(blck["t"].ToString()[0]);
                            sc.TeacherName = blck["u"].ToString();
                            sc.RoomName = blck["r"].ToString();
                            sc.SubjectShortcut = blck["s"].ToString();
                            sc.SubjectName = blck["k"].ToString();
                            sc.StudyGroups = blck["g"].ToString().Split(',').Select(x => x.Trim()).ToList();
                            sc.SubjectType = (eSubjectType)Convert.ToInt32(blck["p"].ToString());
                            sc.IsEmptyBlock = false;
                        }
                        else
                        {
                            sc.IsEmptyBlock = true;
                        }
                        daySchedule.BlocksInDay.Add(sc);
                    }
                    catch (Exception ex)
                    {
                        //to do logger
                        Console.WriteLine(ex.ToString());
                    }
                    
                }
                ret.DaysInWeek.Add(daySchedule);
            }
            return ret;
        }

        private eLessonType ConvertLessonType(char lessonShortcutType)
        {
            switch (lessonShortcutType)
            {
                case 'L': return eLessonType.Laboratory;
                case 'P': return eLessonType.Lecture;
                case 'C': return eLessonType.Excercise;
                default: throw new ArgumentException($"Unexpected lesson type '{lessonShortcutType}'");    
            }
        }
    }

}
