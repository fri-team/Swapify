using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities.Enums;
using WebAPI.Models.UserModels;
using System.Collections.Generic;
using WebAPI.Models.TimetableModels;
using Timetable = WebAPI.Models.TimetableModels.Timetable;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {

        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;

        public StudentController(IStudentService studentService, IUserService userService, ICourseService courseService)
        {
            _studentService = studentService;
            _userService = userService;
            _courseService = courseService;
        }


        [HttpGet("getStudentTimetable/{userEmail}")]
        public async Task<IActionResult> GetStudentTimetable(string userEmail)
        {
            User user = await _userService.GetUserByEmailAsync(userEmail);
            string studentId = user.Student.Id.ToString();
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Student id: {studentId} is not valid GUID.");
            }

            //var student = await _studentService.FindByIdAsync(guid);
            var student = user.Student;
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

            foreach (var block in user.Student.Timetable.AllBlocks)
            {
                TimetableBlock timetableBlock = new TimetableBlock();
                Course course = await _courseService.FindByIdAsync(block.CourseId);
                timetableBlock.Id = block.CourseId.ToString();
                timetableBlock.Day = block.Day.GetHashCode();
                timetableBlock.StartBlock = block.StartHour - 6;
                timetableBlock.EndBlock = timetableBlock.StartBlock + block.Duration;
                timetableBlock.CourseName = course.CourseName;
                timetableBlock.CourseShortcut = course.CourseCode;
                timetableBlock.Room = block.Room;
                timetableBlock.Teacher = block.Teacher;
                timetableBlock.Type = (TimetableBlockType)block.BlockType;
                Blocks.Add(timetableBlock);
            }
            timetable.Blocks = Blocks;

            return Ok(timetable);
        }

        [HttpPost("addNewBlock")]
        public async Task<IActionResult> AddNewBlock([FromBody]AddNewBlockModel newBlockModel)
        {
            var _user = await _userService.GetUserByEmailAsync(newBlockModel.User.Email);

            var student = _user.Student;

            if (student == null)
            {
                return ErrorResponse($"Student with id: {student.Id} does not exist.");
            }

            if (student.Timetable == null)
            {
                return ErrorResponse($"Timetable for student with id: {student.Id} does not exist.");
            }

            student.Timetable.AddNewBlock(newBlockModel.Block);
            await _studentService.UpdateStudentAsync(student);
            //return block with new id 
            return Ok(newBlockModel.Block);
        }

        
        [HttpDelete("{studentId}/blocks/{day}/{teacher}/{room}/{startHour}/{duration}/{type}")]
        public async Task<IActionResult> RemoveBlock(string studentId,
                                                     Day day,
                                                     string teacher,
                                                     string room,
                                                     byte startHour,
                                                     byte duration,
                                                     BlockType type)
        {
            Block block = new Block(type, day, startHour, duration, room, teacher);

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


            if (student.Timetable.RemoveBlock(block))
            {
                await _studentService.UpdateStudentAsync(student);
            }
            else
            {
                return ErrorResponse($"Block {block.ToString()} does not exist in student {studentId} timetable.");
            }
            return Ok();
        }
    }
}
