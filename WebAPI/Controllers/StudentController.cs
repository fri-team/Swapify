
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {

        private ILogger<UserController> _logger;
        private readonly IStudentService _studentService;

        public StudentController(ILogger<UserController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }


        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentTimetable(string studentId)
        {
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return BadRequest(new ErrorMessage($"Student id: {studentId} is not valid GUID."));
            }

            var student = await _studentService.FindByIdAsync(guid);

            if (student == null)
            {
                return BadRequest(new ErrorMessage($"Student with id: {studentId} does not exist."));
            }

            if (student.Timetable == null)
            {
                return BadRequest(new ErrorMessage($"Timetable for student with id: {studentId} does not exist."));
            }

            return Ok(student.Timetable);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBlock([FromBody]string studentId, [FromBody]Block block)
        {
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return BadRequest(new ErrorMessage($"Student id: {studentId} is not valid GUID."));
            }

            var student = await _studentService.FindByIdAsync(guid);

            if (student == null)
            {
                return BadRequest(new ErrorMessage($"Student with id: {studentId} does not exist."));
            }

            if (student.Timetable == null)
            {
                return BadRequest(new ErrorMessage($"Timetable for student with id: {studentId} does not exist."));
            }

            student.Timetable.AddNewBlock(block);
            await _studentService.UpdateStudentAsync(student);
            //return block with new id 
            return Ok(block);
        }

     
        [HttpPost]
        public async Task<IActionResult> RemoveBlock([FromBody]string studentId, [FromBody]Block block)
        {
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return BadRequest(new ErrorMessage($"Student id: {studentId} is not valid GUID."));
            }

            var student = await _studentService.FindByIdAsync(guid);

            if (student == null)
            {
                return BadRequest(new ErrorMessage($"Student with id: {studentId} does not exist."));
            }

            if (student.Timetable == null)
            {
                return BadRequest(new ErrorMessage($"Timetable for student with id: {studentId} does not exist."));
            }


            if (student.Timetable.RemoveBlock(block))
            {
                await _studentService.UpdateStudentAsync(student);
            }
            else
            {
                return BadRequest(new ErrorMessage($"Block {block.ToString()} does not exist in student {studentId} timetable."));
            }

            return Ok();
        }
    }
}
