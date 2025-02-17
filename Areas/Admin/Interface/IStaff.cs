using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStaff
    {

        Task<BaseApiResponseModel> GetAllStaff();
        Task<BaseApiResponseModel> GetStaffById(int id);
        Task<BaseApiResponseModel> InsertUpdateStaff(StaffModel model);
        Task<BaseApiResponseModel> DeleteStaff(int id);
    }
}
