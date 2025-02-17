using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICourseRepository
    {

        Task<BaseApiResponseModel> GetAllCourse();
        Task<BaseApiResponseModel> GetCourseById(int id);
        Task<BaseApiResponseModel> AddCourse(CourseModel model);
        Task<BaseApiResponseModel> UpdateCourse(int courseId, CourseModel model);
        Task<BaseApiResponseModel> DeleteCourse(int id);
    }
}
