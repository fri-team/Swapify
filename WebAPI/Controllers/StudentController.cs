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
        private readonly ITimetableDataService _timetableDataService;
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly ISchoolScheduleProxy _schoolScheduleProxy;
        private readonly IBlockChangesService _blockChangesService;
        private readonly ICalendarService _calendarService;

        public StudentController(ILogger<TimetableController> logger,
            ITimetableDataService timetableDataService,
            IUserService userService,
            ICourseService courseService,
            ISchoolScheduleProxy schoolScheduleProxy,
            IBlockChangesService blockChangesService,
            ICalendarService calendarService)
        {
            _logger = logger;
            _timetableDataService = timetableDataService;
            _userService = userService;
            _courseService = courseService;
            _schoolScheduleProxy = schoolScheduleProxy;
            _blockChangesService = blockChangesService;
            _calendarService = calendarService;
        }


        [HttpGet("getStudentTimetable/{userEmail}")]
        public async Task<IActionResult> GetUserTimetable(string userEmail)
        {
            try
            {
                _logger.LogInformation($"[API getStudentTimetable] Setting timetable for test timetableData");
                User user = await _userService.GetUserByEmailAsync(userEmail);
                if (user?.TimetableData == null)
                {
                    _logger.Log(LogLevel.Information, $"StudentTimetable for user: {userEmail} does not exist.");
                    return Ok(new Timetable());
                }
                string timetableDataId = user.TimetableData.Id.ToString();
                if (!Guid.TryParse(timetableDataId, out Guid guid))
                {
                    _logger.Log(LogLevel.Error, $"Timetable data id: {timetableDataId} is not valid GUID.");
                    return ErrorResponse($"Timetable data id: {timetableDataId} is not valid GUID.");
                }
                var timetableData = await _timetableDataService.FindByIdAsync(guid);
                if (timetableData == null)
                {
                    _logger.Log(LogLevel.Error, $"Timetable data with id: {timetableDataId} does not exist.");
                    return ErrorResponse($"Timetable data with id: {timetableDataId} does not exist.");
                }
                if (timetableData.Timetable == null)
                {
                    _logger.Log(LogLevel.Error, $"Timetable for timetableData with id: {timetableDataId} does not exist.");
                    return Ok(new Timetable());
                }
                if (timetableData.Timetable.IsOutDated() && !string.IsNullOrEmpty(timetableData.PersonalNumber))
                {
                    var scheduleTimetable = await _schoolScheduleProxy.GetByPersonalNumber(timetableData.PersonalNumber, timetableData.TimetableType);
                    if (scheduleTimetable == null) return ErrorResponse($"Timetable data with number: {timetableData.PersonalNumber} does not exist.");
                    await _timetableDataService.UpdateTimetableAsync(timetableData,
                        await ConverterApiToDomain.ConvertTimetableForPersonalNumberAsync(scheduleTimetable, _courseService, user.TimetableData.ShowBlockedHours)
                    );
                    await _userService.UpdateUserAsync(user);
                    var requests = await _blockChangesService.FindWaitingStudentRequests(timetableData.Id);
                    foreach (var item in requests)
                    {
                        await _blockChangesService.CancelExchangeRequest(item);
                    }
                }
                var timetable = new Timetable();
                var Blocks = new List<TimetableBlock>();
                foreach (var block in timetableData.Timetable.AllBlocks)
                {
                    TimetableBlock timetableBlock = new TimetableBlock();
                    Course course = await _courseService.FindByIdAsync(block.CourseId);
                    if (course != null)
                    {
                        timetableBlock.Id = block.BlockId.ToString();
                        timetableBlock.Day = block.Day.GetHashCode();
                        timetableBlock.StartBlock = block.StartHour - 6;
                        timetableBlock.EndBlock = timetableBlock.StartBlock + block.Duration;
                        timetableBlock.CourseId = course.Id.ToString();
                        timetableBlock.CourseName = course.CourseName;
                        timetableBlock.CourseCode = course.CourseCode;
                        timetableBlock.CourseShortcut = course.CourseShortcut;
                        timetableBlock.Room = block.Room;
                        timetableBlock.Teacher = block.Teacher;
                        timetableBlock.Type = (TimetableBlockType)block.BlockType;
                        timetableBlock.BlockColor = block.BlockColor;
                        Blocks.Add(timetableBlock);
                    }
                }
                timetable.Blocks = Blocks;
                return Ok(timetable);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"When processing user: {userEmail} exception was invoked: {e}");
                return ErrorResponse($"StudentTimetable: {userEmail} produced exception.");
            }
        }

        [HttpPost("addNewBlock")]
        public async Task<IActionResult> AddNewBlock([FromBody] AddNewBlockModel newBlockModel)
        {
            var _user = await _userService.GetUserByEmailAsync(newBlockModel.User.Email);
            var timetableData = await _timetableDataService.FindByIdAsync(_user.TimetableData.Id);

            if (timetableData == null)
            {
                return ErrorResponse($"Timetable data with id: {_user.TimetableData.Id} does not exist.");
            }

            if (timetableData.Timetable == null)
            {
                return ErrorResponse($"Timetable for timetableData with id: {timetableData.Id} does not exist.");
            }

            TimetableBlock timetableBlock = newBlockModel.TimetableBlock;
            Course course = await _courseService.GetOrAddNotExistsCourse(timetableBlock.CourseCode,
                timetableBlock.CourseName, timetableBlock.CourseShortcut);

            if (course == null)
            {
                return ErrorResponse($"Course: {timetableBlock.CourseName} does not exist.");
            }

            Block block = TimetableBlock.ConvertToBlock(timetableBlock, course.Id);

            if (timetableData.Timetable.IsSubjectPresentInTimetable(block))
            {
                return ErrorResponse($"Course: {timetableBlock.CourseName} is already present.");
            }

            timetableData.Timetable.AddNewBlock(block);
            timetableData.Timetable.UpdateColorOfBlocksWithSameCourseId(block);
            await _timetableDataService.UpdateTimetableDataAsync(timetableData);
            //return block with new id 
            return Ok(newBlockModel.TimetableBlock);
        }

        [HttpDelete("removeBlock/{userEmail}/{blockId}")]
        public async Task<IActionResult> RemoveBlock(string userEmail, string blockId)
        {
            var _user = await _userService.GetUserByEmailAsync(userEmail);
            bool isValidGUID = Guid.TryParse(_user.TimetableData.Id.ToString(), out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Timetable data id: {_user.TimetableData.Id.ToString()} is not valid GUID.");
            }

            var timetableData = await _timetableDataService.FindByIdAsync(guid);
            if (timetableData == null)
            {
                return ErrorResponse($"Timetable data with id: {_user.TimetableData.Id.ToString()} does not exist.");
            }
            if (timetableData.Timetable == null)
            {
                return ErrorResponse($"Timetable for timetableData with id: {timetableData.Id} does not exist.");
            }

            Guid blockGuid = Guid.Parse(blockId);
            if (timetableData.Timetable.RemoveBlock(blockGuid))
            {
                await _timetableDataService.UpdateTimetableDataAsync(timetableData);
            }
            else
            {
                return ErrorResponse($"Block with id {blockId} does not exist in timetableData {timetableData.Id} timetable.");
            }
            return Ok();
        }

        [HttpPut("editBlock")]
        public async Task<IActionResult> EditBlock([FromBody] UpdateBlockModel updateBlockModel)
        {
            var _user = await _userService.GetUserByEmailAsync(updateBlockModel.User.Email);
            bool isValidGUID = Guid.TryParse(_user.TimetableData.Id.ToString(), out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Timetable data id: {_user.TimetableData.Id.ToString()} is not valid GUID.");
            }

            var timetableData = await _timetableDataService.FindByIdAsync(guid);
            if (timetableData == null)
            {
                return ErrorResponse($"Timetable data with id: {updateBlockModel.User.TimetableData.Id} does not exist.");
            }

            if (timetableData.Timetable == null)
            {
                return ErrorResponse($"Timetable for timetableData with id: {timetableData.Id} does not exist.");
            }

            Course newCourse = await _courseService.FindByCodeAsync(updateBlockModel.TimetableBlock.CourseCode);
            if (newCourse == null)
            {
                return ErrorResponse($"New course: {updateBlockModel.TimetableBlock.CourseName} does not exist.");
            }

            Block newBlock = TimetableBlock.ConvertToBlock(updateBlockModel.TimetableBlock, newCourse.Id);

            if (timetableData.Timetable.UpdateBlock(newBlock))
            {
                timetableData.Timetable.UpdateColorOfBlocksWithSameCourseId(newBlock);
                await _timetableDataService.UpdateTimetableDataAsync(timetableData);
            }
            else
            {
                return ErrorResponse($"Block {newBlock.ToString()} is not updated");
            }

            return Ok(newBlock);
        }

        [HttpGet("getStudentTimetableCalendar/{userEmail}")]
        public async Task<IActionResult> GetUserTimetableCalendar(string userEmail)
        {
            _logger.LogInformation($"[API getStudentTimetable] Getting timetable calendar for test timetableData");
            User user = await _userService.GetUserByEmailAsync(userEmail);
            string timetableDataId = user.TimetableData.Id.ToString();
            bool isValidGUID = Guid.TryParse(timetableDataId, out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Timetable data id: {timetableDataId} is not valid GUID.");
            }

            var timetableData = await _timetableDataService.FindByIdAsync(guid);
            if (timetableData == null)
            {
                return ErrorResponse($"Timetable data with id: {timetableDataId} does not exist.");
            }

            if (timetableData.Timetable == null)
            {
                return ErrorResponse($"Timetable for timetable data with id: {timetableDataId} does not exist.");
            }

            StringBuilder sb = _calendarService.StartCalendar();

            foreach (var block in timetableData.Timetable.AllBlocks)
            {
                Course course = await _courseService.FindByIdAsync(block.CourseId);
                sb = _calendarService.CreateEvent(sb, block, course);
            }

            sb = _calendarService.EndCalendar(sb);

            return Ok(sb.ToString());
        }
    }
}
