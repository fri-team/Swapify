using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Models.TimetableModels;
using WebAPI.Models.UserModels;
using Timetable = WebAPI.Models.TimetableModels.Timetable;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TimetableController : BaseController
    {
        private readonly ILogger<TimetableController> _logger;
        private readonly ISchoolScheduleProxy _schoolScheduleProxy;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;        
        public TimetableController(ILogger<TimetableController> logger, ISchoolScheduleProxy schoolScheduleProxy,
            IStudentService studentService, IUserService userService, ICourseService courseService)
        {
            _logger = logger;
            _schoolScheduleProxy = schoolScheduleProxy;
            _studentService = studentService;
            _userService = userService;
            _courseService = courseService;
        }

        [HttpPost("initializeTimetableForTestUsers")]
        public async Task<IActionResult> InitializeTimetableForTestUsers([FromBody] StudentModel body)
        {
            _logger.LogInformation($"[TEST] Setting test timetable for test student: {body.Email}.");
            User user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogError($"[TEST] Test user with email: {body.Email} does not exist.");
                return ErrorResponse($"User with email: {body.Email} does not exist.");
            }
            var timetable = _schoolScheduleProxy.GetFromJsonFile("TestTimetable.json");
            if (timetable == null)
                return ErrorResponse("Loading of test timetable failed.");

            FRITeam.Swapify.Entities.Timetable studentTimetable = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(timetable, _courseService);

            Student student = user.Student;
            if (student == null)
            {
                student = new Student
                {
                    PersonalNumber = "000000",
                    UserId = user.Id
                };
                await _studentService.AddAsync(student);

                user.Student = student;

                await _studentService.UpdateStudentTimetableAsync(student, studentTimetable);
                await _userService.UpdateUserAsync(user);
                return Ok(student.Timetable);
            }
            if (student.PersonalNumber != null && student.PersonalNumber.Equals("000000"))
            {
                return Ok(student.Timetable);
            }
            else
            {
                student.PersonalNumber = "000000";
                await _studentService.UpdateStudentTimetableAsync(student, studentTimetable);
                await _userService.UpdateUserAsync(user);
                return Ok(student.Timetable);
            }
        }

        [HttpPost("setStudentTimetableFromPersonalNumber")]
        public async Task<IActionResult> SetStudentTimetableFromPersonalNumber([FromBody] StudentModel body)
        {
            _logger.LogInformation($"Setting timetable for student: {body.Email}.");
            User user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogError($"User with email: {body.Email} does not exist.");
                return ErrorResponse($"User with email: {body.Email} does not exist.");
            }
            var timetable = _schoolScheduleProxy.GetByPersonalNumber(body.PersonalNumber);
            if (timetable == null) return ErrorResponse($"Student with number: {body.PersonalNumber} does not exist.");

            FRITeam.Swapify.Entities.Timetable studentTimetable = await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(timetable, _courseService);            
            Student student = user.Student;
            if (student == null)
            {
                student = new Student
                {
                    PersonalNumber = body.PersonalNumber,
                    UserId = user.Id
                };
                await _studentService.AddAsync(student);

                user.Student = student;

                await _studentService.UpdateStudentTimetableAsync(student, studentTimetable);
                await _userService.UpdateUserAsync(user);
                return Ok(student.Timetable);
            }
            if (student.PersonalNumber != null && student.PersonalNumber.Equals(body.PersonalNumber))
            {
                return Ok(student.Timetable);
            }
            else
            {
                student.PersonalNumber = body.PersonalNumber;
                await _studentService.UpdateStudentTimetableAsync(student, studentTimetable);
                await _userService.UpdateUserAsync(user);
                return Ok(student.Timetable);
            }
        }

        [HttpGet("course/getCoursesAutoComplete/{courseName}/{studentId}")]
        public async Task<IActionResult> GetCoursesAutoComplete(string courseName, string studentId)
        {
            Guid.TryParse(studentId, out Guid studentGuid);
            var _student = await _studentService.FindByIdAsync(studentGuid);

            return Ok(_courseService.FindByStartName(courseName, _student != null ? _student.PersonalNumber : "5"));
        }

        [HttpGet("getCourseTimetable/{courseId}/{studentId}")]
        public async Task<IActionResult> GetCourseTimetable(string courseId, string studentId)
        {
            bool isValidCourseGUID = Guid.TryParse(courseId, out Guid courseGuid);            
            if (!isValidCourseGUID)
            {
                return ErrorResponse($"Course id: {courseId} is not valid GUID.");
            }            

            bool isValidStudentGUID = Guid.TryParse(studentId, out Guid studentGuid);
            var _student = await _studentService.FindByIdAsync(studentGuid);

            if (!isValidStudentGUID)
            {
                return ErrorResponse($"Student id: {studentGuid} is not valid GUID.");
            }
            if (_student == null)
            {
                return ErrorResponse($"Student with id: {studentId} does not exist.");
            }
            var _course = await _courseService.FindCourseTimetableFromProxy(courseGuid);
            if (_course == null)
            {
                return ErrorResponse($"Course with id: {courseId} does not exist.");
            }            
            var timetable = new Timetable();
            var Blocks = new List<TimetableBlock>();
            foreach (var block in _course.Timetable.AllBlocks)
            {
                TimetableBlock timetableBlock = new TimetableBlock
                {
                    Id = block.BlockId.ToString(),
                    Day = block.Day.GetHashCode(),
                    StartBlock = block.StartHour - 6,
                    EndBlock = block.StartHour - 6 + block.Duration,
                    CourseId = _course.Id.ToString(),
                    CourseName = _course.CourseName,
                    CourseCode = _course.CourseCode ?? "",
                    Room = block.Room,
                    Teacher = block.Teacher,
                    Type = (TimetableBlockType)block.BlockType
                };
                if (!_student.Timetable.ContainsBlock(block))
                    Blocks.Add(timetableBlock);
            }
            timetable.Blocks = Blocks;

            return Ok(timetable);
        }

        [HttpGet("getCourseBlock/{courseId}/{startBlock}/{day}")]
        public async Task<IActionResult> GetBlockOfCourse(string courseId, int startBlock, int day)
        {
            if (!Guid.TryParse(courseId, out Guid courseGuid))
            {
                return ErrorResponse($"Course id: {courseId} is not valid GUID.");
            }

            var _course = await _courseService.FindCourseTimetableFromProxy(courseGuid);
            if (_course == null)
            {
                return ErrorResponse($"Course with code: {courseId} does not exist.");
            }

            foreach (var block in _course.Timetable.AllBlocks)
            {
                if (block.StartHour == startBlock && block.Day.GetHashCode() == day)
                {
                    return Ok(new TimetableBlock
                    {
                        Id = block.BlockId.ToString(),
                        Day = block.Day.GetHashCode(),
                        StartBlock = block.StartHour - 6,
                        EndBlock = block.StartHour - 6 + block.Duration,
                        CourseId = _course.Id.ToString(),
                        CourseName = _course.CourseName,
                        CourseCode = _course.CourseCode ?? "",
                        Room = block.Room,
                        Teacher = block.Teacher,
                        Type = (TimetableBlockType)block.BlockType
                    });
                }                       
            }
            return NotFound("Course not found");
        }
    }
}
