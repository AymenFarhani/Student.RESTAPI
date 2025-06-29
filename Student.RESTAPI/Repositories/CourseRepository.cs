using Microsoft.EntityFrameworkCore;
using Student.RESTAPI.Data;
using Student.RESTAPI.Models;

namespace Student.RESTAPI.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _dbContext;

        public CourseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _dbContext.Courses.FindAsync(id);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _dbContext.Courses.ToListAsync();
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            _dbContext.Courses.Add(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null) return false;

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            var student = await _dbContext.Students.FindAsync(studentId);
            var course = await _dbContext.Courses.FindAsync(courseId);

            if (student == null || course == null) return false;

            var enrollment = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow
            };

            _dbContext.StudentCourses.Add(enrollment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnenrollStudentAsync(int studentId, int courseId)
        {
            var enrollment = await _dbContext.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (enrollment == null) return false;

            _dbContext.StudentCourses.Remove(enrollment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _dbContext.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)
                .Select(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Student>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _dbContext.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .Include(sc => sc.Student)
                .Select(sc => sc.Student)
                .ToListAsync();
        }
    }
}
