using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStudent
    {
<<<<<<< HEAD
        Task<List<StudentGETModel>> GetAllStudent();
        Task<StudentGETModel> GetStudentById(int id);
        Task<bool> InsertUpdateStudent(StudentModel model);
=======
        Task<List<StudentViewModel>> GetAllStudent();
        Task<StudentViewModel> GetStudentById(int id);
        Task<bool> InsertUpdateStudent(StudentViewModel model);
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        Task<bool> DeleteStudent(int id);
    }
}
