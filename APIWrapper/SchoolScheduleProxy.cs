using FRITeam.Swapify.APIWrapper.Objects;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolScheduleProxy : ISchoolScheduleProxy
    {
        private const string URL = "https://nic.uniza.sk/webservices";
        private const string SCHEDULE_CONTENT_URL = "getUnizaScheduleContent.php";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<ScheduleHourContent> GetByPersonalNumber(string personalNumber)
        {
            string semester = getCurrentSemesterShortCut();
            return CallScheduleContentApi(5, personalNumber + semester);
        }

        public IEnumerable<ScheduleHourContent> GetByTeacherName(string teacherNumber)
        {
            return CallScheduleContentApi(1, teacherNumber);
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
            string address =  $"{URL}/{SCHEDULE_CONTENT_URL}?m={type}&id={Uri.EscapeUriString(requestContent)}";
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
                throw;
            }
            return ResponseParser.ParseResponse(myResponse);
        }

        private string getCurrentSemesterShortCut()
        {
            DateTime localDate = DateTime.Now;
            DateTime winterSemesterStart = new DateTime(localDate.Year, 9, 1); //start of winter semester 1.9.
            DateTime winterSemesterEnd = new DateTime(localDate.Year, 3, 1); //end of winter semester and start of summer semester 1.3.
            DateTime summerSemesterEnd = new DateTime(localDate.Year, 7, 1); //end of summer semester 1.7.

            //if current semester is winter semester <1.9.; 1.3.)
            if (localDate.CompareTo(winterSemesterStart) != -1 && localDate.CompareTo(winterSemesterEnd) == -1)
            {
                return $"&r={localDate.Year}&s='Z'";
            } //if current semester is summer semester <1.3.; 1.7.)
            else if (localDate.CompareTo(winterSemesterEnd) != -1 && localDate.CompareTo(summerSemesterEnd) == -1)
            {
                return $"&r={localDate.Year}&s='L'";
            }

            return "";
        }
    }

}
