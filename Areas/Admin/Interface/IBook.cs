using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBook
    {

        Task<List<BookGETModel>> GetAllBook();
        Task<BookGETModel> GetBookById(int id);
        Task<bool> InsertUpdateBook(BookModel model);
        Task<bool> DeleteBook(int id);
    }
}
