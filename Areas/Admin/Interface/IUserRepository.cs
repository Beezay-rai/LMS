using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IUserRepository
    {
        Task<BaseApiResponseModel> SignUpUser(SignUpModel model);

        Task<BaseApiResponseModel> GetAllUser();



    }
}
