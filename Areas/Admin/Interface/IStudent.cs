using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStudent
    {
        Task<List<StudentViewModel>> GetAllStudent();
        Task<StudentViewModel> GetStudentById(int id);
        Task<bool> InsertUpdateStudent(StudentViewModel model);
        Task<bool> DeleteStudent(int id);
    }
}
