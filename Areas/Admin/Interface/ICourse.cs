using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICourse
    {

        Task<List<CourseModel>> GetAllCourse();
        Task<CourseModel> GetCourseById(int id);
        Task<bool> AddCourse(POSTCourseModel model);
        Task<bool> UpdateCourse(int courseId, POSTCourseModel model);
        Task<bool> DeleteCourse(int id);
    }
}
