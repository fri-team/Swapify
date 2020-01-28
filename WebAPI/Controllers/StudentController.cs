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
                if (course.CourseCode == null)
                {
                    timetableBlock.CourseShortcut = "";
                }
                else
                {
                    timetableBlock.CourseShortcut = course.CourseCode;
                }

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
            Course course = await _courseService.FindByNameAsync(timetableBlock.CourseName);

            if (course == null)
            {
                return ErrorResponse($"Course: {timetableBlock.CourseName} does not exist.");
            }

            Block block = TimetableBlock.ConvertToBlock(timetableBlock, course.Id);

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

            Course newCourse = await _courseService.FindByNameAsync(updateBlockModel.TimetableBlock.CourseName);
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
