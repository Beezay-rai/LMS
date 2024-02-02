using LMS.Data;
using LMS.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public AccountRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<ApiResponse> SignUp(SignUpModel model)
        {
            var response = new ApiResponse();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var newUser = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                    };
                    var createUser = await _userManager.CreateAsync(newUser, model.Password);
                    var addToRole = await _userManager.AddToRoleAsync(newUser, model.Role);
                    if (createUser.Succeeded && addToRole.Succeeded)
                    {
                        response.Status = true;
                        response.Message = "Successfully Created !";
                        await transaction.CommitAsync();
                       
                    }
                    else
                    {
                        response.Status = false;
                        response.Data = createUser.Errors.Select(x => x.Description).Concat(  addToRole.Errors.Select(x=>x.Description)) ;
                        response.Message = "Error in creating new user !" ;
                        await transaction.RollbackAsync();

                    }
                    return response;
                 
                }
                catch (Exception ex)
                {
                    response.Status = false;
                    response.Message = ex.Message;
                    await transaction.RollbackAsync();
                    return response;
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
