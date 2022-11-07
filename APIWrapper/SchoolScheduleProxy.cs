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
            }
            else if (studyType.Contains("inž"))
            {
                numStudyType = '1';
            }
            else if (studyType.Contains("dokt"))
            {
                numStudyType = '3';
            }
            else
            {
                numStudyType = ' ';
            }
            string addition = "," + getFacultyCode(subjectCode) + "," + numStudyType + "," + yearOfStudy;
            return await CallScheduleContentApi(4, subjectCode + addition, Semester.GetSemester());
        }

        //First number in subject code is identifier for corresponding faculty so everyone know to which faculty selected subject belongs
        //Every FRI subject starts with number 6. Before new acreditation every subject on FRI was starting with number 5 (2021/2022)
        //Starting numbers have changed for few faculties but API call parameters to get subject time table was kept same as before
        // that is why we have to specify which number to use for which faculty
        public char getFacultyCode(string subjectCode)
        {
            switch (subjectCode[0])
            {
                case '1':   //FPEDAS = 1
                    return '1';
                case '2':   //Strojnicka = 2
                    return '2';
                case '3':   //FEIT = 3
                    return '3';
                case '4':   //Stavebna = 4
                    return '4';
                case '5':   //FBI = 9
                    return '9';
                case '6':   //FRI = 5
                    return '5';
                case '7':   //Humanitna = 8
                    return '8';
                case '8':   //Ustav vysokohorskej biologie = 6
                    return '6';
                default:
                    return ' ';
            }
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
