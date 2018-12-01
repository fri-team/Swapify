using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.TimetableModels;
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

        public TimetableController(IStudyGroupService groupService,
                                   ICourseService courseService,
                                   ISchoolScheduleProxy proxy,
                                   IStudentService studentService
                                   )
        {
            _groupService = groupService;
            _courseService = courseService;
            _proxy = proxy;
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> SetStudentTimetableFromGroup(string studyGroupNumber, User user)
        {
            StudyGroup sg = await _groupService.GetStudyGroupAsync(studyGroupNumber, _courseService, _proxy);
            if (sg == null)
            {
                return ErrorResponse($"Study group with number: {studyGroupNumber} does not exist.");
            }
            Student student = new Student();
            student.StudyGroupId = sg.Id;
            student.Timetable = sg.Timetable;
            user.Student = student;

            await _studentService.UpdateStudentTimetableAsync(student, sg);
            return Ok(student.Timetable);
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
