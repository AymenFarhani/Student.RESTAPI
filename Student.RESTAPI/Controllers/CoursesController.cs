using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.RESTAPI.Models;
using Student.RESTAPI.Services;

namespace Student.RESTAPI.Controllers
{
    [Route("api/v1/courses")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdCourse = await _courseService.AddCourseAsync(course);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (id != course.Id) return BadRequest("ID mismatch");

            var updatedCourse = await _courseService.UpdateCourseAsync(course);
            return Ok(updatedCourse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{courseId}/enroll/{studentId}")]
        public async Task<IActionResult> EnrollStudent(int courseId, int studentId)
        {
            var result = await _courseService.EnrollStudentAsync(studentId, courseId);
            if (!result) return BadRequest("Enrollment failed");
            return Ok();
        }

        [HttpPost("{courseId}/unenroll/{studentId}")]
        public async Task<IActionResult> UnenrollStudent(int courseId, int studentId)
        {
            var result = await _courseService.UnenrollStudentAsync(studentId, courseId);
            if (!result) return BadRequest("Unenrollment failed");
            return Ok();
        }

        [HttpGet("{id}/students")]
        public async Task<IActionResult> GetStudentsByCourseId(int id)
        {
            var students = await _courseService.GetStudentsByCourseIdAsync(id);
            return Ok(students);
        }
    }
}
