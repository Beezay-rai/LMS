using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IAuthor
    {

        Task<List<AuthorViewModel>> GetAllAuthor();
        Task<AuthorViewModel> GetAuthorById(int id);
        Task<bool> InsertUpdateAuthor(AuthorViewModel model);
        Task<bool> DeleteAuthor(int id);
    }
}
