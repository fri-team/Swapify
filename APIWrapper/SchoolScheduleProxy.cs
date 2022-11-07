using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using FRITeam.Swapify.SwapifyBase.Settings.ProxySettings;
using Microsoft.Extensions.Options;
using NLog;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolScheduleProxy : ISchoolScheduleProxy
    {        
        private const string TYPE_PARAM = "m";
        private const string ID_PARAM = "id";
        private const string YEAR_PARAM = "r";
        private const string SEMESTER_PARAM = "s";

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly RestClient _client;
        private readonly ProxySettings _proxySettings;

        public SchoolScheduleProxy(IOptions<ProxySettings> proxySettings)
        {
            _proxySettings = proxySettings.Value;
            _client = new RestClient(_proxySettings.Base_URL).UseNewtonsoftJson();            
        }

        public async Task<ScheduleTimetableResult> GetByPersonalNumber(string personalNumber, UserType userType = UserType.Student)
        {
            if (userType == UserType.Student)
            {
                return await CallScheduleContentApi(5, personalNumber, Semester.GetSemester());
            }
            else if (userType == UserType.Teacher)
                return await CallScheduleContentApi(1, personalNumber, Semester.GetSemester());
            else
                throw new ArgumentException($"Unknown user type: {userType}");
        }

        public async Task<ScheduleTimetableResult> GetByTeacherName(string teacherNumber)
        {
            return await CallScheduleContentApi(1, teacherNumber, Semester.GetSemester());
        }

        public async Task<ScheduleTimetableResult> GetByRoomNumber(string roomNumber)
        {
            return await CallScheduleContentApi(3, roomNumber, Semester.GetSemester());
        }

        public async Task<ScheduleTimetableResult> GetBySubjectCode(string subjectCode, string yearOfStudy, string studyType)
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
            string addition = "," + subjectCode[0] + "," + numStudyType + "," + yearOfStudy;           
            return await CallScheduleContentApi(4, subjectCode + addition, Semester.GetSemester());
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

        private async Task<ScheduleTimetableResult> CallScheduleContentApi(int type, string id, Semester semester)
        {            
            var request = new RestRequest(_proxySettings.ScheduleContentURL);
            request.AddParameter(TYPE_PARAM, type);
            request.AddParameter(ID_PARAM, id);
            request.AddParameter(YEAR_PARAM, semester.Year);
            request.AddParameter(SEMESTER_PARAM, (char)semester.SemesterValue);
            try
            {
                var response = await _client.GetAsync<UnizaScheduleContentResult>(request);
                return new ScheduleTimetableResult()
                {
                    Semester = semester,
                    Result = response
                };
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        }                
    }

}
