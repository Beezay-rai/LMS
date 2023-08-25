using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IIssueBook
    {

        Task<List<IssueBookGETViewModel>> GetAllIssueBook();
        Task<IssueBookGETViewModel> GetIssueBookById(int id);
        Task<bool> InsertUpdateIssueBook(IssueBookViewModel model);
        Task<bool> DeleteIssueBook(int id);
    }
}
