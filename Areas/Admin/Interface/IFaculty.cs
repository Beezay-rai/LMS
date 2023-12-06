using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IFaculty
    {

        Task<List<FacultyViewModel>> GetAllFaculty();
        Task<FacultyViewModel> GetFacultyById(int id);
        Task<bool> InsertUpdateFaculty(FacultyViewModel model);
        Task<bool> DeleteFaculty(int id);
    }
}
