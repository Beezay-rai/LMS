using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStudent
    {

        Task<BaseApiResponseModel> GetAllStudent();
        Task<BaseApiResponseModel> GetStudentById(int id);
        Task<BaseApiResponseModel> AddStudent(StudentModel model);
        Task<BaseApiResponseModel> UpdateStudent(int courseId, StudentModel model);
        Task<BaseApiResponseModel> DeleteStudent(int id);
    }
}
