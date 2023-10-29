using LMS.Data;
using LMS.Interface;
using LMS.Models;
using LMS.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<bool> SignUp(SignUpModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                    };
                    var abc = await _userManager.CreateAsync(user, model.Password);
                    if (abc.Succeeded)
                    {
                        var b = await _userManager.AddToRoleAsync(user, model.Role);
                        await transaction.CommitAsync();
                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }

        }

        public async Task<string> Login(LoginModel model)
        {
            var user = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!user.Succeeded)
            {
                return "Login Failure! Please try again!";
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authLoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authLoginKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
