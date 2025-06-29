using Student.RESTAPI.Models;

namespace Student.RESTAPI.Repositories
{
    public interface ICourseRepository
    {
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> AddCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int id);
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
        Task<bool> UnenrollStudentAsync(int studentId, int courseId);
        Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId);
        Task<IEnumerable<Models.Student>> GetStudentsByCourseIdAsync(int courseId);
    }
}
