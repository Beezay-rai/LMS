using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IAuthor
    {

        Task<List<AuthorViewModel>> GetAllAuthor();
        Task<AuthorViewModel> GetAuthorById(int id);
        Task<bool> CreateAuthor(AuthorViewModel model);
        Task<bool> EditAuthor(AuthorViewModel model);
        Task<bool> DeleteAuthor(int id);
    }
}
