using LMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LMS.Interface
{
    public interface IAccount
    {
        Task<ApiResponse> SignUp(SignUpModel model);
        Task<ApiResponse> Login(LoginModel model);
        Task<ApiResponse> GoogleLogin(string crediantialToken);
        Task<ApiResponse> GoogleSignUp(string crediantialToken);
    }
}
