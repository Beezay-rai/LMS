using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ICategory
    {

<<<<<<< HEAD
        Task<List<CategoryModel>> GetAllCategory();
        Task<CategoryModel> GetCategoryById(int id);
        Task<bool> InsertUpdateCategory(CategoryModel model);
=======
        Task<List<CategoryViewModel>> GetAllCategory();
        Task<CategoryViewModel> GetCategoryById(int id);
        Task<bool> InsertUpdateCategory(CategoryViewModel model);
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        Task<bool> DeleteCategory(int id);
    }
}
