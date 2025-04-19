using LMS.Models;

namespace LMS.Interfaces
{
    public interface IAuthenticateRepository
    {
        Task<BaseApiResponseModel> SignUp(SignUpModel model);
        Task<BaseApiResponseModel> Login(LoginModel model);
        Task<BaseApiResponseModel> GoogleLogin(string crediantialToken);
        Task<BaseApiResponseModel> GoogleSignUp(string crediantialToken);
    }
}
