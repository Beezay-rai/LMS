using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBook
    {

        Task<BaseApiResponseModel> GetAllBook();
        Task<BaseApiResponseModel> GetBookById(int id);
        Task<BaseApiResponseModel> AddBook(BookModel model);
        Task<BaseApiResponseModel> UpdateBook(int book_id, BookModel model);
        Task<BaseApiResponseModel> DeleteBook(int id);
    }
}
