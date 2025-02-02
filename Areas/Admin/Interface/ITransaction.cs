using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface ITransaction
    {

        Task<BaseApiResponseModel> GetAllTransaction();
        Task<BaseApiResponseModel> GetTransactionById(int id);
        Task<BaseApiResponseModel> InsertUpdateTransaction(TransactionModel model);
        Task<BaseApiResponseModel> DeleteTransaction(int id);
    }
}
