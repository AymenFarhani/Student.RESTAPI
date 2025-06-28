using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student.RESTAPI.Services;

namespace Student.RESTAPI.Controllers
{
    [Route("api/v1/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Models.Student student)
        {
            if (student == null)
            {
                return BadRequest("Student data is null");
            }
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Models.Student student)
        {
            if (student == null)
            {
                return BadRequest("Student data is invalid");
            }

            if (id != student.Id)
            {
                return BadRequest("ID in route doesn't match student ID");
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudentAsync(student);
                return Ok(updatedStudent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("name")]
        public async Task<IActionResult> SearchByName(
       [FromQuery] string name,
       [FromQuery] bool countOnly = false,
       [FromQuery] int? skip = null,
       [FromQuery] int? take = null)
        {
            try
            {
                var query = _studentService.SearchByName(name);

                if (countOnly)
                {
                    return Ok(await query.CountAsync());
                }

                if (skip.HasValue)
                {
                    query = query.Skip(skip.Value);
                }

                if (take.HasValue)
                {
                    query = query.Take(take.Value);
                }

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("birthdate-before")]
        public async Task<IActionResult> SearchByBirthDateBefore(
       [FromQuery] DateOnly date,
       [FromQuery] bool countOnly = false,
       [FromQuery] int? skip = null,
       [FromQuery] int? take = null)
        {
            var query = _studentService.SearchByBirthDateBefore(date);

            if (countOnly)
            {
                return Ok(await query.CountAsync());
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }

    }
}
