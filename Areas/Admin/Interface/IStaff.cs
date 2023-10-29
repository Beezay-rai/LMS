using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStaff
    {

        Task<List<StaffModel>> GetAllStaff();
        Task<StaffModel> GetStaffById(int id);
        Task<bool> InsertUpdateStaff(StaffModel model);
        Task<bool> DeleteStaff(int id);
    }
}
