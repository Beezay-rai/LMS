using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBook
    {
        Task<List<BookViewModel>> GetAllBook();
        Task<BookViewModel> GetBookById(int id);
        Task<bool> CreateBook(BookViewModel model);
        Task<bool> EditBook(BookViewModel model);
        Task<bool> DeleteBook(int id);


    }
}
