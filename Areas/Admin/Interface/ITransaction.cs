using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ITransaction
    {

        Task<List<TransactionGETModel>> GetAllTransaction();
        Task<TransactionGETModel> GetTransactionById(int id);
        Task<bool> InsertUpdateTransaction(TransactionModel model);
        Task<bool> DeleteTransaction(int id);
    }
}
