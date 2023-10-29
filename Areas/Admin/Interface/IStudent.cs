using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStudent
    {
        Task<List<StudentModel>> GetAllStudent();
        Task<StudentModel> GetStudentById(int id);
        Task<bool> InsertUpdateStudent(StudentModel model);
        Task<bool> DeleteStudent(int id);
    }
}
