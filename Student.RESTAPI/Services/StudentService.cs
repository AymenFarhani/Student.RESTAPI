
using Student.RESTAPI.Repositories;

namespace Student.RESTAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<Models.Student> CreateStudentAsync(Models.Student student)
        {
            return await _studentRepository.CreateStudentAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepository.DeleteStudentAsync(id);
        }

        public async Task<IEnumerable<Models.Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Models.Student> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetStudentByIdAsync(id);
        }

        public IQueryable<Models.Student> SearchByBirthDateBefore(DateOnly date)
        {
            return _studentRepository.SearchByBirthDateBefore(date);
        }

        public IQueryable<Models.Student> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return _studentRepository.GetAllStudentsAsync().Result.AsQueryable();
            }

            return _studentRepository.SearchByName(name);
        }

        public async Task<Models.Student> UpdateStudentAsync(Models.Student student)
        {
            // First check if student exists
            var existingStudent = await _studentRepository.GetStudentByIdAsync(student.Id);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException($"Student with ID {student.Id} not found");
            }

            // Update properties
            existingStudent.FullName = student.FullName;
            existingStudent.Email = student.Email;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Address = student.Address;

            return await _studentRepository.UpdateStudentAsync(existingStudent);
        }
    }
}
