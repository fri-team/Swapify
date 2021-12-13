using FRITeam.Swapify.Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.Models.UserModels;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using WebAPI.Models.TimetableModels;
using Timetable = WebAPI.Models.TimetableModels.Timetable;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.SwapifyBase.Entities;
using System.Text;

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
        private readonly ISchoolScheduleProxy _schoolScheduleProxy;
        private readonly IBlockChangesService _blockChangesService;
        private readonly ICalendarService _calendarService;

        public StudentController(ILogger<TimetableController> logger,
            IStudentService studentService,
            IUserService userService,
            ICourseService courseService,
            ISchoolScheduleProxy schoolScheduleProxy,
            IBlockChangesService blockChangesService,
            ICalendarService calendarService)
        {
            _logger = logger;
            _studentService = studentService;
            _userService = userService;
            _courseService = courseService;
            _schoolScheduleProxy = schoolScheduleProxy;
            _blockChangesService = blockChangesService;
            _calendarService = calendarService;
        }


        [HttpGet("getStudentTimetable/{userEmail}")]
        public async Task<IActionResult> GetStudentTimetable(string userEmail)
        {
            try
            {
                _logger.LogInformation($"[API getStudentTimetable] Setting timetable for test student");
                User user = await _userService.GetUserByEmailAsync(userEmail);
                if (user?.Student == null)
                {
                    _logger.Log(LogLevel.Information, $"Student for user: {userEmail} does not exist.");
                    return Ok(new Timetable());
                }
                string studentId = user.Student.Id.ToString();
                if (!Guid.TryParse(studentId, out Guid guid))
                {
                    _logger.Log(LogLevel.Error, $"Student id: {studentId} is not valid GUID.");
                    return ErrorResponse($"Student id: {studentId} is not valid GUID.");
                }
                var student = await _studentService.FindByIdAsync(guid);
                if (student == null)
                {
                    _logger.Log(LogLevel.Error, $"Student with id: {studentId} does not exist.");
                    return ErrorResponse($"Student with id: {studentId} does not exist.");
                }
                if (student.Timetable == null)
                {
                    _logger.Log(LogLevel.Error, $"Timetable for student with id: {studentId} does not exist.");
                    return Ok(new Timetable());
                }
                if (student.Timetable.IsOutDated() && !string.IsNullOrEmpty(student.PersonalNumber))
                {
                    var scheduleTimetable = await _schoolScheduleProxy.GetByPersonalNumber(student.PersonalNumber);
                    if (scheduleTimetable == null) return ErrorResponse($"Student with number: {student.PersonalNumber} does not exist.");
                    await _studentService.UpdateStudentTimetableAsync(student,
                        await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(scheduleTimetable, _courseService)
                    );
                    await _userService.UpdateUserAsync(user);
                    var requests = await _blockChangesService.FindWaitingStudentRequests(student.Id);
                    foreach (var item in requests)
                    {
                        await _blockChangesService.CancelExchangeRequest(item);
                    }
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
                    timetableBlock.BlockColor = block.BlockColor;
                    Blocks.Add(timetableBlock);
                }
                timetable.Blocks = Blocks;
                return Ok(timetable);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"When processing user: {userEmail} exception was invoked: {e}");
                return ErrorResponse($"Student: {userEmail} produced exception.");
            }
        }

        [HttpPost("addNewBlock")]
        public async Task<IActionResult> AddNewBlock([FromBody] AddNewBlockModel newBlockModel)
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
            student.Timetable.UpdateColorOfBlocksWithSameCourseId(block);
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
                student.Timetable.UpdateColorOfBlocksWithSameCourseId(newBlock);
                await _studentService.UpdateStudentAsync(student);
            }
            else
            {
                return ErrorResponse($"Block {newBlock.ToString()} is not updated");
            }

            return Ok(newBlock);
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

            StringBuilder sb = _calendarService.StartCalendar();

            foreach (var block in student.Timetable.AllBlocks)
            {
                Course course = await _courseService.FindByIdAsync(block.CourseId);
                sb = _calendarService.CreateEvent(sb, block, course);
            }

            sb = _calendarService.EndCalendar(sb);

            return Ok(sb.ToString());
        }
    }
}
