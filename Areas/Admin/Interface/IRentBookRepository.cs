using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IRentBookRepository
    {

        Task<BaseApiResponseModel> GetAllRentBook();
        Task<BaseApiResponseModel> GetRentBookById(int id);
        Task<BaseApiResponseModel> AddRentBook(RentBookModel model);
        Task<BaseApiResponseModel> UpdateRentBook(int RentBook_id, RentBookModel model);
        Task<BaseApiResponseModel> DeleteRentBook(int id);
    }
}
