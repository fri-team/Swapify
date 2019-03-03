using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IStudyGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;

        public TimetableController(ILogger<TimetableController> logger, IStudyGroupService groupService,
            IStudentService studentService, IUserService userService, ICourseService courseService)
        {
            _logger = logger;
            _groupService = groupService;
            _studentService = studentService;
            _userService = userService;
            _courseService = courseService;
        }

        [HttpPost("setStudentTimetableFromGroup")]
        public async Task<IActionResult> SetStudentTimetableFromGroup([FromBody] StudentModel body)
        {
            _logger.LogInformation($"Setting timetable for student: {body.Email}.");
            User user = await _userService.GetUserByEmailAsync(body.Email);
            if (user == null)
            {
                _logger.LogError($"User with email: {body.Email} does not exist.");
                return ErrorResponse($"User with email: {body.Email} does not exist.");
            }
            StudyGroup sg = await _groupService.GetStudyGroupAsync(body.GroupNumber);
            if (sg == null)
                return ErrorResponse($"Study group with number: {body.GroupNumber} does not exist.");

            Student student = user.Student;
            if (student == null)
            {
                student = new Student();
                await _studentService.AddAsync(student);

                user.Student = student;
                await _studentService.UpdateStudentTimetableAsync(student, sg);

                await _userService.UpdateUserAsync(user);
                return Ok(student.Timetable);
            }
            if (student.StudyGroup.Equals(sg))
            {
                return Ok(student.Timetable);
            }
            else
            {
                await _studentService.UpdateStudentTimetableAsync(student, sg);
                await _userService.UpdateUserAsync(user);
                return Ok(student.Timetable);
            }
        }

        [HttpGet("course/getCoursesAutoComplete/{courseName}")]
        public IActionResult GetCoursesAutoComplete(string courseName)
        {
            return Ok(this._courseService.FindByStartName(courseName));
        }

        [HttpGet("getCourseTimetable/{courseId}")]
        public async Task<IActionResult> GetCourseTimetable(string courseId)
        {
            bool isValidGUID = Guid.TryParse(courseId, out Guid guid);
            //var _course = await _courseService.FindByIdAsync(guid);
            Course _course = await _courseService.FindCourseTimetableFromProxy(guid);

            if (!isValidGUID)
            {
                return ErrorResponse($"Course id: {courseId} is not valid GUID.");
            }
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
                    Id = _course.Id.ToString(),
                    Day = block.Day.GetHashCode(),
                    StartBlock = block.StartHour - 6,
                    EndBlock = block.StartHour - 6 + block.Duration,
                    CourseName = _course.CourseName,
                    CourseShortcut = _course.CourseCode ?? "",
                    Room = block.Room,
                    Teacher = block.Teacher,
                    Type = (TimetableBlockType)block.BlockType
                };
                Blocks.Add(timetableBlock);
            }
            timetable.Blocks = Blocks;

            return Ok(timetable);
        }
    }
}
