namespace Student.RESTAPI.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Models.Student>> GetAllStudentsAsync();
        Task<Models.Student> GetStudentByIdAsync(int id);
        Task<Models.Student> CreateStudentAsync(Models.Student student);
        Task<Models.Student> UpdateStudentAsync(Models.Student student);
        Task<bool> DeleteStudentAsync(int id);

        IQueryable<Models.Student> SearchByName(string name);
        IQueryable<Models.Student> SearchByBirthDateBefore(DateOnly date);
    }
}
