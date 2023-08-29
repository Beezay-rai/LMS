using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICategory
    {

        Task<List<CategoryViewModel>> GetAllCategory();
        Task<CategoryViewModel> GetCategoryById(int id);
        Task<bool> InsertUpdateCategory(CategoryViewModel model);
        Task<bool> DeleteCategory(int id);
    }
}
