using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICategory
    {

        Task<BaseApiResponseModel> GetAllCategory();
        Task<BaseApiResponseModel> GetCategoryById(int id);
        Task<BaseApiResponseModel> AddCategory(CategoryModel model);
        Task<BaseApiResponseModel> UpdateCategory(int courseId, CategoryModel model);
        Task<BaseApiResponseModel> DeleteCategory(int id);
    }
}
