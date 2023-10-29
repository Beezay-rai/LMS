using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IFaculty
    {

        Task<List<FacultyModel>> GetAllFaculty();
        Task<FacultyModel> GetFacultyById(int id);
        Task<bool> InsertUpdateFaculty(FacultyModel model);
        Task<bool> DeleteFaculty(int id);
    }
}
