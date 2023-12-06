using LMS.Models;

namespace LMS.Interface
{
    public interface ICommon
    {
        Task<List<CommonModel>> GetGender();
        Task<List<CommonModel>> GetRoles();
        Task<List<GETSignUpModel>> GetAllUser();
    }
}