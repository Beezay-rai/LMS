using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICategory
    {

        Task<List<CategoryModel>> GetAllCategory();
        Task<CategoryModel> GetCategoryById(int id);
        Task<bool> AddCourse(POSTCategoryModel model);
        Task<bool> UpdateCourse(int courseId, POSTCategoryModel model);
        Task<bool> DeleteCategory(int id);
    }
}
