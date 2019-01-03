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

        public TimetableController(ILogger<TimetableController> logger, IStudyGroupService groupService,
            IStudentService studentService, IUserService userService)
        {
            _logger = logger;
            _groupService = groupService;
            _studentService = studentService;
            _userService = userService;
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

        [HttpGet("course/{courseId}")]
        public IActionResult GetCourseTimetable(string courseId)
        {
            if (!string.Equals(courseId, "DISS"))
            {
                return ErrorResponse($"Course with id: {courseId} does not exist.");
            }
            var timetable = new Timetable
            {
                Blocks = new List<TimetableBlock>
                {
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 1,
                        StartBlock = 1,
                        EndBlock = 3,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "RB054",
                        Teacher = "Peter Jankovič",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 1,
                        StartBlock = 4,
                        EndBlock = 6,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "RB054",
                        Teacher = "Peter Jankovič",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 1,
                        StartBlock = 7,
                        EndBlock = 9,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "RB054",
                        Teacher = "Peter Jankovič",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 3,
                        StartBlock = 7,
                        EndBlock = 9,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "RB054",
                        Teacher = "Boris Bučko",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 4,
                        StartBlock = 4,
                        EndBlock = 6,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "RB054",
                        Teacher = "Boris Bučko",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 4,
                        StartBlock = 11,
                        EndBlock = 13,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "AF3A6",
                        Teacher = "Norbert Adamko",
                        Type = TimetableBlockType.Lecture
                    },
                    new TimetableBlock
                    {
                        Id= Guid.NewGuid().ToString(),
                        Day = 5,
                        StartBlock = 4,
                        EndBlock = 6,
                        CourseName = "Diskrétna simulácia",
                        CourseShortcut = "DISS",
                        Room = "RB054",
                        Teacher = "Boris Bučko",
                        Type = TimetableBlockType.Laboratory
                    }
                }
            };
            return Ok(timetable);
        }
    }
}
