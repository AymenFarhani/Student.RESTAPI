
using Microsoft.EntityFrameworkCore;
using Student.RESTAPI.Data;

namespace Student.RESTAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _dbContext;

        public StudentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Student> CreateStudentAsync(Models.Student student)
        {
            return await Task.Run(() =>
            {
                _dbContext.Students.Add(student);
                _dbContext.SaveChanges();
                return student;
            });
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await Task.Run(() =>
            {
                var student = _dbContext.Students.AsNoTracking().FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return false;
                }
                _dbContext.Students.Remove(student);
                return _dbContext.SaveChanges() > 0;
            });
        }

        public async Task<IEnumerable<Models.Student>> GetAllStudentsAsync()
        {
            return await _dbContext.Students.AsNoTracking().ToListAsync();
        }

        public async Task<Models.Student> GetStudentByIdAsync(int id)
        {
            return await _dbContext.Students.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IQueryable<Models.Student> SearchByBirthDateBefore(DateOnly date)
        {
            return _dbContext.Students
            .Where(s => s.DateOfBirth < date);
        }

        public IQueryable<Models.Student> SearchByName(string name)
        {
            return _dbContext.Students
            .Where(s => s.FullName.Contains(name));
        }

        public async Task<Models.Student> UpdateStudentAsync(Models.Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
            return student;
        }
    }
}
