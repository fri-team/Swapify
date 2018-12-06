using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.TimetableModels;
using WebAPI.Models.UserModels;
using Timetable = WebAPI.Models.TimetableModels.Timetable;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TimetableController : BaseController
    {
        private readonly IStudyGroupService _groupService;
        private readonly ICourseService _courseService;
        private readonly ISchoolScheduleProxy _proxy;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public TimetableController(IStudyGroupService groupService,
                                   ICourseService courseService,
                                   ISchoolScheduleProxy proxy,
                                   IStudentService studentService,
                                   IUserService userService
                                   )
        {
            _groupService = groupService;
            _courseService = courseService;
            _proxy = proxy;
            _studentService = studentService;
            _userService = userService;
        }

        [HttpPost("setStudentTimetableFromGroup")]
        public async Task<IActionResult> SetStudentTimetableFromGroup([FromBody] StudentModel body)
        {
            StudyGroup sg = await _groupService.GetStudyGroupAsync(body.GroupNumber, _courseService, _proxy);
            User user = await _userService.GetUserAsync(body.user.Email);
            if (sg == null)
            {
                return ErrorResponse($"Study group with number: {body.GroupNumber} does not exist.");
            }
            else if (user == null)
            {
                return ErrorResponse($"User with email: {body.user.Email} does not exist.");
            }
            Student student = user.Student;
            if (student == null)
            {
                student = new Student();
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
