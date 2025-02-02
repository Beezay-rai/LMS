using LMS.Models;

namespace LMS.Interface
{
    public interface IAccount
    {
        Task<BaseApiResponseModel> SignUp(SignUpModel model);
        Task<BaseApiResponseModel> Login(LoginModel model);
        Task<BaseApiResponseModel> GoogleLogin(string crediantialToken);
        Task<BaseApiResponseModel> GoogleSignUp(string crediantialToken);
    }
}
