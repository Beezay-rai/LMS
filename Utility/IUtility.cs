using LMS.Models;
using System.Security.Claims;

namespace LMS.Utility
{
    public interface IUtility
    {
        Task<List<Claim>> GetUserClaims(ApplicationUser user);
        Task<ApplicationUser> GetUserById(string userId);

    }
}
