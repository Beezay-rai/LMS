using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IStaff
    {

        Task<List<StaffViewModel>> GetAllStaff();
        Task<StaffViewModel> GetStaffById(int id);
        Task<bool> InsertUpdateStaff(StaffViewModel model);
        Task<bool> DeleteStaff(int id);
    }
}
