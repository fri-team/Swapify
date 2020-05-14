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
            string addition = ",5,1,1" + getCurrentSemesterShortCut();
            return CallScheduleContentApi(4, subjectCode + addition);
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
            DateTime endOfTheYear = new DateTime(localDate.Year, 12, 31, 23,59,59); //end of the year
            DateTime newYear = new DateTime(localDate.Year, 1, 1); //end of the year
            DateTime winterSemesterEnd = new DateTime(localDate.Year, 2, 15); //end of winter semester and start of summer semester 15.2.
            DateTime summerSemesterEnd = new DateTime(localDate.Year, 7, 1); //end of summer semester 1.7.

            //if current semester is winter semester <1.9.; 31.12.>
            if (localDate.CompareTo(winterSemesterStart) != -1 && localDate.CompareTo(endOfTheYear) == -1)
                return $"&r={localDate.Year}&s=Z";

            //if current semester is winter semester <1.1.; 15.2.)
            if (localDate.CompareTo(newYear) != -1 && localDate.CompareTo(winterSemesterEnd) == -1)
                return $"&r={localDate.Year - 1}&s=Z";

            //if current semester is summer semester <15.2.; 1.7.)
            if (localDate.CompareTo(winterSemesterEnd) != -1 && localDate.CompareTo(summerSemesterEnd) == -1)
                return $"&r={localDate.Year - 1}&s=L";

            return "";
        }
    }

}
