using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities.Enums;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {

        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public StudentController(IStudentService studentService, IUserService userService)
        {
            _studentService = studentService;
            _userService = userService;
        }


        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentTimetable(string studentId)
        {
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

            return Ok(student.Timetable);
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
