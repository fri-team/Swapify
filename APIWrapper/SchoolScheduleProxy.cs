using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Entities;
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

        public ScheduleTimetable GetByPersonalNumber(string personalNumber)
        {
            Semester semester = Semester.GetSemester();
            return CallScheduleContentApi(5, personalNumber + FormatSemesterForApi(semester), semester);
        }

        public ScheduleTimetable GetByTeacherName(string teacherNumber)
        {
            return CallScheduleContentApi(1, teacherNumber, Semester.GetSemester());
        }

        public ScheduleTimetable GetByRoomNumber(string roomNumber)
        {
            return CallScheduleContentApi(3, roomNumber, Semester.GetSemester());
        }

        public ScheduleTimetable GetBySubjectCode(string subjectCode, string yearOfStudy, string studyType)
        {
            char numStudyType;
            if (string.IsNullOrEmpty(studyType))
            {
                numStudyType = ' ';
            }
            else if (studyType.Contains("bak"))
            {
                numStudyType = '2';
            } else if (studyType.Contains("inž"))
            {
                numStudyType = '1';
            } else if (studyType.Contains("dokt"))
            {
                numStudyType = '3';
            } else
            {
                numStudyType = ' ';
            }             
            string addition = "," + subjectCode[0] + "," + numStudyType + "," + yearOfStudy + FormatSemesterForApi(Semester.GetSemester());
            return CallScheduleContentApi(4, subjectCode + addition, Semester.GetSemester());
        }

        public ScheduleTimetable GetFromJsonFile(string fileName)
        {
            var myJson = "";
            using (StreamReader file = File.OpenText($@"..\Tests\{fileName}"))
            {
                myJson = file.ReadToEnd();
            }
            return new ScheduleTimetable()
            {
                ScheduleHourContents = ResponseParser.ParseResponse(myJson),
                Semester = Semester.GetSemester()
            };
        }

        private ScheduleTimetable CallScheduleContentApi(int type, string requestContent, Semester semester)
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
            return new ScheduleTimetable()
            {
                ScheduleHourContents = ResponseParser.ParseResponse(myResponse),
                Semester = semester
            };
        }        
        private string FormatSemesterForApi(Semester semester)
        {
            if (semester == null)
            {
                return string.Empty;
            }
            return $"&r={semester.Year}&s={(char)semester.SemesterValue}";
        }
    }

}
