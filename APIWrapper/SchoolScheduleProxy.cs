using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Entities;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolScheduleProxy : ISchoolScheduleProxy
    {
        private const string URL = "https://nic.uniza.sk/webservices";
        private const string SCHEDULE_CONTENT_URL = "getUnizaScheduleContent.php";
        private const string TYPE_PARAM = "m";
        private const string ID_PARAM = "id";

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<ScheduleTimetableResult> GetByPersonalNumber(string personalNumber)
        {
            Semester semester = Semester.GetSemester();
            var client = new RestClient(URL);
            var request = new RestRequest(SCHEDULE_CONTENT_URL);
            request.AddHeader("Accept", "application/json");                        
            request.AddParameter(TYPE_PARAM, 5);
            request.AddParameter(ID_PARAM, Uri.EscapeUriString(personalNumber + FormatSemesterForApi(semester)));
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            try
            {
                var response = await client.GetAsync<UnizaScheduleContentResult>(request);
                return new ScheduleTimetableResult()
                {
                    Semester = semester,
                    Result = response
                };
            }
            catch (Exception e)
            {
                _logger.Info(e.Message);
                throw;
            }                        

            //return CallScheduleContentApi(5, personalNumber + FormatSemesterForApi(semester), semester);
        }

        public ScheduleTimetableResult GetByTeacherName(string teacherNumber)
        {
            return CallScheduleContentApi(1, teacherNumber, Semester.GetSemester());
        }

        public ScheduleTimetableResult GetByRoomNumber(string roomNumber)
        {
            return CallScheduleContentApi(3, roomNumber, Semester.GetSemester());
        }

        public ScheduleTimetableResult GetBySubjectCode(string subjectCode, string yearOfStudy, string studyType)
        {
            char numStudyType;
            if (studyType.Contains("Celoživotné vzdelávanie"))
            {
                numStudyType = subjectCode[3] == '2' ? '1' : '2';
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

        public ScheduleTimetableResult GetFromJsonFile(string fileName)
        {
            var myJson = "";
            using (StreamReader file = File.OpenText($@"..\Tests\{fileName}"))
            {
                myJson = file.ReadToEnd();
            }
            return new ScheduleTimetableResult()
            {
                Result = new UnizaScheduleContentResult()
                {
                    ScheduleContents = ResponseParser.ParseResponse(myJson)
                },
                Semester = Semester.GetSemester()
            };
        }

        private ScheduleTimetableResult CallScheduleContentApi(int type, string requestContent, Semester semester)
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
            return new ScheduleTimetableResult()
            {
                //ScheduleHourContents = ResponseParser.ParseResponse(myResponse),
                Result = null,
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
