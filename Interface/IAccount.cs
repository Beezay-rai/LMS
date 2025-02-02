using LMS.Models;

namespace LMS.Interface
{
    public interface IAccount
    {
        Task<ApiResponseModel> SignUp(SignUpModel model);
        Task<ApiResponseModel> Login(LoginModel model);
        Task<ApiResponseModel> GoogleLogin(string crediantialToken);
        Task<ApiResponseModel> GoogleSignUp(string crediantialToken);
    }
}
