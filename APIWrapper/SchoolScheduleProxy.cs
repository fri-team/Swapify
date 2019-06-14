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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<ScheduleHourContent> GetByTeacherName(string teacherNumber)
        {
            return CallScheduleContentApi(1, teacherNumber);
        }

        public IEnumerable<ScheduleHourContent> GetByStudyGroup(string studyGroupNumber)
        {
            return CallScheduleContentApi(2, studyGroupNumber);
        }

        public IEnumerable<ScheduleHourContent> GetByRoomNumber(string roomNumber)
        {
            return CallScheduleContentApi(3, roomNumber);
        }

        public IEnumerable<ScheduleHourContent> GetBySubjectCode(string subjectCode)
        {
            return CallScheduleContentApi(4, subjectCode);
        }

        private IEnumerable<ScheduleHourContent> CallScheduleContentApi(int type, string requestContent)
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
            return ResponseParser.ParseResponse(myResponse);
        }

    }

}
