using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICategory
    {

        Task<List<CategoryModel>> GetAllCategory();
        Task<CategoryModel> GetCategoryById(int id);
        Task<bool> InsertUpdateCategory(CategoryModel model);
        Task<bool> DeleteCategory(int id);
    }
}
