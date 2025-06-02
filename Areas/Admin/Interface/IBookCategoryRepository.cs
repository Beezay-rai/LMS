using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBookCategoryRepository
    {
        Task<BaseApiResponseModel> GetAllCategory();
        Task<BaseApiResponseModel> GetCategoryById(int id);
        Task<BaseApiResponseModel> AddCategory(BookCategoryModel model);
        Task<BaseApiResponseModel> UpdateCategory(int courseId, BookCategoryModel model);
        Task<BaseApiResponseModel> DeleteCategory(int id);
    }
}
