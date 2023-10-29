using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IIssueBook
    {

        Task<List<IssueBookGETModel>> GetAllIssueBook();
        Task<IssueBookGETModel> GetIssueBookById(int id);
        Task<bool> InsertUpdateIssueBook(IssueBookModel model);
        Task<bool> DeleteIssueBook(int id);
        Task<bool> ReturnedIssuedBook(int id,bool status);
    }
}
