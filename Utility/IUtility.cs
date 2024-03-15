using LMS.Models;
using System.Security.Claims;

namespace LMS.Utility
{
    public interface IUtility
    {


        Task<List<Claim>> GetUserClaims(ApplicationUser user);
        string GenerateJWTToken(List<Claim> authclaims);

    }
}
