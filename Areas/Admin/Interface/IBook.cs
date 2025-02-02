using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBook
    {

        Task<List<BookModel>> GetAllBook();
        Task<BookModel> GetBookById(int id);
        Task<bool> AddBook(BookModel model);
        Task<bool> UpdateBook(int book_id, BookModel model);
        Task<bool> DeleteBook(int id);
    }
}
