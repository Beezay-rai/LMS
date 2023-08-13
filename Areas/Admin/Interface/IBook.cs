using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBook
    {

        Task<List<BookViewModel>> GetAllBook();
        Task<BookViewModel> GetBookById(int id);
        Task<bool> InsertUpdateBook(BookViewModel model);
        Task<bool> DeleteBook(int id);
    }
}
