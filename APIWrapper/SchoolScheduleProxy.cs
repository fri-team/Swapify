using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolScheduleProxy : ISchoolScheduleProxy
    {
        private const string __URL__ = "https://nic.uniza.sk/webservices";
        private const string __SCHEDULE_CONTENT_URL__ = "getUnizaScheduleContent.php";
        
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
            string address = $"{__URL__}/{__SCHEDULE_CONTENT_URL__}?m={type}&id={Uri.EscapeUriString(requestContent)}";
            string myResponse = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

            }
            return ParseResponse(myResponse);
        }

        private ScheduleWeekContent ParseResponse(string myResponse)
        {
            
            JObject joResponse = JObject.Parse(myResponse);
            //check if error occured
            string report = joResponse["report"].ToString();
            if (!string.IsNullOrWhiteSpace(report))
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
                        
                        if (!string.IsNullOrWhiteSpace(blck["t"].ToString()) && !string.IsNullOrWhiteSpace(blck["p"].ToString()))
                        { 
                            var isBlocked = Convert.ToBoolean(int.Parse(blck["b"].ToString()));
                            var lessonType = this.ConvertLessonType(blck["t"].ToString()[0]);
                            var teacherName = blck["u"].ToString();
                            var roomName = blck["r"].ToString();
                            var subjectShortcut = blck["s"].ToString();
                            var subjectName = blck["k"].ToString();
                            var studyGroups = blck["g"].ToString().Split(',').Select(x => x.Trim()).ToList();
                            var subjectType = (SubjectType)Convert.ToInt32(blck["p"].ToString());
                            sc = new ScheduleHourContent(blockNumber++, isBlocked,lessonType,teacherName,roomName,subjectShortcut,subjectName,subjectType);
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
