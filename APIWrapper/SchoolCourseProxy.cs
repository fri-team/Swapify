using FRITeam.Swapify.APIWrapper.Objects;
using NLog;
using RestSharp;
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
            var response = "";            
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest(SCHEDULE_CONTENT_URL);
                request.AddParameter("q", courseName);
                response = client.Get(request).Content;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);                
                throw;
            }            
            return ResponseParser.ParseCourseResponse(response);            
        }  
    }
}
