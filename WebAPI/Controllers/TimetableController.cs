using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TimetableController : Controller
    {
        [HttpGet]
        public IActionResult GetStudentTimetable()
        {
            var timetable = new Timetable
            {
                Blocks = new List<TimetableBlock>
                {
                    new TimetableBlock
                    {
                        Day = 1,
                        StartBlock = 7,
                        EndBlock = 9,
                        CourseName = "Teória informácie",
                        CourseShortcut = "TI",
                        Room = "RA301",
                        Teacher = "Tomáš Majer",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Day = 2,
                        StartBlock = 6,
                        EndBlock = 8,
                        CourseName = "Teória informácie",
                        CourseShortcut = "TI",
                        Room = "RC009",
                        Teacher = "Stanislav Palúch",
                        Type = TimetableBlockType.Lecture
                    },
                    new TimetableBlock
                    {
                        Day = 3,
                        StartBlock = 8,
                        EndBlock = 10,
                        CourseName = "Architektúry informačných systémov",
                        CourseShortcut = "AIS",
                        Room = "RB003",
                        Teacher = "Matilda Drozdová",
                        Type = TimetableBlockType.Laboratory
                    },
                    new TimetableBlock
                    {
                        Day = 4,
                        StartBlock = 1,
                        EndBlock = 3,
                        CourseName = "Databázy a získavanie znalostí",
                        CourseShortcut = "II05",
                        Room = "RC009",
                        Teacher = "Vitaly Levashenko",
                        Type = TimetableBlockType.Lecture
                    },
                    new TimetableBlock
                    {
                        Day = 4,
                        StartBlock = 6,
                        EndBlock = 8,
                        CourseName = "Architektúry informačných systémov",
                        CourseShortcut = "AIS",
                        Room = "RC009",
                        Teacher = "Matilda Drozdová",
                        Type = TimetableBlockType.Lecture
                    },
                    new TimetableBlock
                    {
                        Day = 4,
                        StartBlock = 9,
                        EndBlock = 11,
                        CourseName = "Teória spoľahlivosti",
                        CourseShortcut = "TSP",
                        Room = "RA201",
                        Teacher = "Elena Zaitseva",
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
                        StartBlock = 2,
                        EndBlock = 4,
                        CourseName = "Teória spoľahlivosti",
                        CourseShortcut = "TSP",
                        Room = "RA201",
                        Teacher = "Elena Zaitseva",
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
                    },
                    new TimetableBlock
                    {
                        Day = 5,
                        StartBlock = 6,
                        EndBlock = 8,
                        CourseName = "Databázy a získavanie znalostí",
                        CourseShortcut = "II05",
                        Room = "RB052",
                        Teacher = "Vitaly Levashenko",
                        Type = TimetableBlockType.Laboratory
                    }
                }
            };
            return Ok(timetable);
        }

        [HttpGet("course/{courseId}")]
        public IActionResult GetCourseTimetable(string courseId)
        {
            if (!string.Equals(courseId, "DISS"))
            {
                return BadRequest(new ErrorMessage($"Course with id: {courseId} does not exist."));
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
