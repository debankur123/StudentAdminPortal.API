using StudentsAdminPortal.API.Domain.Models;

namespace StudentsAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentAsync(int id);
        Task<List<Gender>> GetGenderAsync();
        Task<UpdateStudentRecords> UpdateStudentDetails(int id, UpdateStudentRecords _records);
        
    }
}
