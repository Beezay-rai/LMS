using LMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LMS.Interface
{
    public interface IAccount
    {
        Task<IdentityResult> SignUp(SignUpModel model);
        Task<string> Login(LoginModel model);
    }
}
