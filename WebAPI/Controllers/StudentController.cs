using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities.Enums;
using WebAPI.Models.UserModels;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using WebAPI.Models.TimetableModels;
using Timetable = WebAPI.Models.TimetableModels.Timetable;
using System.Text;
using System.IO;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {
        private readonly ILogger<TimetableController> _logger;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;

        public StudentController(ILogger<TimetableController> logger, IStudentService studentService, IUserService userService, ICourseService courseService)
        {
            _logger = logger;
            _studentService = studentService;
            _userService = userService;
            _courseService = courseService;
        }


        [HttpGet("getStudentTimetable/{userEmail}")]
        public async Task<IActionResult> GetStudentTimetable(string userEmail)
        {
            _logger.LogInformation($"[API getStudentTimetable] Setting timetable for test student");
            User user = await _userService.GetUserByEmailAsync(userEmail);

            if (user.Student == null)
            {
                return Ok(new Timetable());
            }

            string studentId = user.Student.Id.ToString();
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Student id: {studentId} is not valid GUID.");
            }

            var student = await _studentService.FindByIdAsync(guid);
            if (student == null)
            {             
                return ErrorResponse($"Student with id: {studentId} does not exist.");
            }

            if (student.Timetable == null)
            {
                return ErrorResponse($"Timetable for student with id: {studentId} does not exist.");
            }

            var timetable = new Timetable();
            var Blocks = new List<TimetableBlock>();

            foreach (var block in student.Timetable.AllBlocks)
            {
                TimetableBlock timetableBlock = new TimetableBlock();
                Course course = await _courseService.FindByIdAsync(block.CourseId);                
                timetableBlock.Id = block.BlockId.ToString();
                timetableBlock.Day = block.Day.GetHashCode();
                timetableBlock.StartBlock = block.StartHour - 6;
                timetableBlock.EndBlock = timetableBlock.StartBlock + block.Duration;
                timetableBlock.CourseId = course.Id.ToString();
                timetableBlock.CourseName = course.CourseName;
                timetableBlock.CourseCode = course.CourseCode;
                timetableBlock.Room = block.Room;
                timetableBlock.Teacher = block.Teacher;
                timetableBlock.Type = (TimetableBlockType)block.BlockType;
                Blocks.Add(timetableBlock);
            }
            timetable.Blocks = Blocks;

            return Ok(timetable);
        }

        [HttpGet("getStudentTimetableCalendar/{userEmail}")]
        public async Task<IActionResult> GetStudentTimetableCalendar(string userEmail)
        {
            _logger.LogInformation($"[API getStudentTimetable] Getting timetable calendar for test student");
            User user = await _userService.GetUserByEmailAsync(userEmail);
            string studentId = user.Student.Id.ToString();
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Student id: {studentId} is not valid GUID.");
            }

            var student = await _studentService.FindByIdAsync(guid);
            if (student == null)
            {
                return ErrorResponse($"Student with id: {studentId} does not exist.");
            }

            if (student.Timetable == null)
            {
                return ErrorResponse($"Timetable for student with id: {studentId} does not exist.");
            }
            DateTime dateNow = DateTime.Now;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:PUBLISH");

            sb.AppendLine("BEGIN:VTIMEZONE");
            sb.AppendLine("TZID:Europe/Amsterdam");
            sb.AppendLine("BEGIN:STANDARD");
            sb.AppendLine("TZOFFSETTO:+0100");
            sb.AppendLine("TZOFFSETFROM:+0100");
            sb.AppendLine("END:STANDARD");
            sb.AppendLine("END:VTIMEZONE");

            var dayOfWeek = 0;
            switch (dateNow.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dayOfWeek = 7;
                    break;
                case DayOfWeek.Monday:
                    dayOfWeek = 1;
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek = 2;
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek = 3;
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek = 4;
                    break;
                case DayOfWeek.Friday:
                    dayOfWeek = 5;
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek = 6;
                    break;
            }

            foreach (var block in student.Timetable.AllBlocks)
            {
                Course course = await _courseService.FindByIdAsync(block.CourseId);

                //add the event
                sb.AppendLine("BEGIN:VEVENT");

                dateNow = DateTime.Now;
                dateNow = dateNow.AddDays(-dayOfWeek + block.Day.GetHashCode())
                    .AddHours(-dateNow.Hour +block.StartHour)
                    .AddMinutes(-dateNow.Minute);

                //with time zone specified
                sb.AppendLine("DTSTART;TZID=Europe/Amsterdam:" + dateNow.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("DTEND;TZID=Europe/Amsterdam:" + dateNow.AddHours(block.Duration).ToString("yyyyMMddTHHmm00"));

                sb.AppendLine("SUMMARY:" + course.CourseName);
                sb.AppendLine("DESCRIPTION:" + block.Room + ", " + block.Teacher);
                sb.AppendLine("PRIORITY:3");
                sb.AppendLine("END:VEVENT");
            }

            //end calendar item
            sb.AppendLine("END:VCALENDAR");

            return Ok(sb.ToString());
        }

        [HttpPost("addNewBlock")]
        public async Task<IActionResult> AddNewBlock([FromBody]AddNewBlockModel newBlockModel)
        {
            var _user = await _userService.GetUserByEmailAsync(newBlockModel.User.Email);
            var student = await _studentService.FindByIdAsync(_user.Student.Id);

            if (student == null)
            {
                return ErrorResponse($"Student with id: {_user.Student.Id} does not exist.");
            }

            if (student.Timetable == null)
            {
                return ErrorResponse($"Timetable for student with id: {student.Id} does not exist.");
            }

            TimetableBlock timetableBlock = newBlockModel.TimetableBlock;
            Course course = await _courseService.GetOrAddNotExistsCourse(timetableBlock.CourseCode,
                timetableBlock.CourseName);

            if (course == null)
            {
                return ErrorResponse($"Course: {timetableBlock.CourseName} does not exist.");
            }

            Block block = TimetableBlock.ConvertToBlock(timetableBlock, course.Id);

            if (student.Timetable.IsSubjectPresentInTimetable(block))
            {
                return ErrorResponse($"Course: {timetableBlock.CourseName} is already present.");
            }

            student.Timetable.AddNewBlock(block);
            await _studentService.UpdateStudentAsync(student);
            //return block with new id 
            return Ok(newBlockModel.TimetableBlock);
        }

        [HttpDelete("removeBlock/{userEmail}/{blockId}")]
        public async Task<IActionResult> RemoveBlock(string userEmail, string blockId)
        {
            var _user = await _userService.GetUserByEmailAsync(userEmail);
            bool isValidGUID = Guid.TryParse(_user.Student.Id.ToString(), out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Student id: {_user.Student.Id.ToString()} is not valid GUID.");
            }

            var student = await _studentService.FindByIdAsync(guid);
            if (student == null)
            {
                return ErrorResponse($"Student with id: {_user.Student.Id.ToString()} does not exist.");
            }
            if (student.Timetable == null)
            {
                return ErrorResponse($"Timetable for student with id: {student.Id} does not exist.");
            }

            Guid blockGuid = Guid.Parse(blockId);
            if (student.Timetable.RemoveBlock(blockGuid))
            {
                await _studentService.UpdateStudentAsync(student);
            }
            else
            {
                return ErrorResponse($"Block with id {blockId} does not exist in student {student.Id} timetable.");
            }
            return Ok();
        }

        [HttpPut("editBlock")]
        public async Task<IActionResult> EditBlock([FromBody] UpdateBlockModel updateBlockModel)
        {
            var _user = await _userService.GetUserByEmailAsync(updateBlockModel.User.Email);
            bool isValidGUID = Guid.TryParse(_user.Student.Id.ToString(), out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Student id: {_user.Student.Id.ToString()} is not valid GUID.");
            }

            var student = await _studentService.FindByIdAsync(guid);
            if (student == null)
            {
                return ErrorResponse($"Student with id: {updateBlockModel.User.Student.Id} does not exist.");
            }

            if (student.Timetable == null)
            {
                return ErrorResponse($"Timetable for student with id: {student.Id} does not exist.");
            }

            Course newCourse = await _courseService.FindByCodeAsync(updateBlockModel.TimetableBlock.CourseCode);
            if (newCourse == null)
            {
                return ErrorResponse($"New course: {updateBlockModel.TimetableBlock.CourseName} does not exist.");
            }

            Block newBlock = TimetableBlock.ConvertToBlock(updateBlockModel.TimetableBlock, newCourse.Id);

            if (student.Timetable.UpdateBlock(newBlock))
            {
                await _studentService.UpdateStudentAsync(student);
            }
            else
            {
                return ErrorResponse($"Block {newBlock.ToString()} is not updated");
            }

            return Ok(newBlock);
        }
    }
}
