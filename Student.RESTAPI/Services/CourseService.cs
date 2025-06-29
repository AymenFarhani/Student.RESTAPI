using Student.RESTAPI.Models;
using Student.RESTAPI.Repositories;

namespace Student.RESTAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;

        public CourseService(ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetCourseByIdAsync(id);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            return await _courseRepository.AddCourseAsync(course);
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            return await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await _courseRepository.DeleteCourseAsync(id);
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            // Additional business logic can be added here
            return await _courseRepository.EnrollStudentAsync(studentId, courseId);
        }

        public async Task<bool> UnenrollStudentAsync(int studentId, int courseId)
        {
            // Additional business logic can be added here
            return await _courseRepository.UnenrollStudentAsync(studentId, courseId);
        }

        public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _courseRepository.GetCoursesByStudentIdAsync(studentId);
        }

        public async Task<IEnumerable<Models.Student>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _courseRepository.GetStudentsByCourseIdAsync(courseId);
        }
    }
}
