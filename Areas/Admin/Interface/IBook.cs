using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IBook
    {

<<<<<<< HEAD
        Task<List<BookGETModel>> GetAllBook();
        Task<BookGETModel> GetBookById(int id);
        Task<bool> InsertUpdateBook(BookModel model);
=======
        Task<List<BookGETViewModel>> GetAllBook();
        Task<BookViewModel> GetBookById(int id);
        Task<bool> InsertUpdateBook(BookViewModel model);
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        Task<bool> DeleteBook(int id);
    }
}
