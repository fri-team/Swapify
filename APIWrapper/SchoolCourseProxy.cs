using FRITeam.Swapify.APIWrapper.Objects;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolCourseProxy : ISchoolCourseProxy
    {
        private const string URL = "https://nic.uniza.sk/webservices";
        private const string SCHEDULE_CONTENT_URL = "getUnizaScheduleType4.php";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<CourseContent> GetByCourseName(string courseName)
        {
            return CallCourseContentApi("q", courseName);
        }

        private IEnumerable<CourseContent> CallCourseContentApi(string type, string requestContent)
        {
            string address = $"{URL}/{SCHEDULE_CONTENT_URL}?{type}={Uri.EscapeUriString(requestContent)}";
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
            return ResponseParser.ParseCourseResponse(myResponse);
        }
    }
}
