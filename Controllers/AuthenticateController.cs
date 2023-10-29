using LMS.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAccount _account;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticateController(IAccount account, UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _account = account;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route("api/SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _account.SignUp(model);
            if (result)
            {
                return Ok(new ApiResponse { Status= true, Message= "Registered Sucessfully !!" } );
            }
            return Ok(new ApiResponse { Status = false, Message = "Failed to Register" });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (user.Succeeded)
            {
                var userCheck = await _userManager.FindByEmailAsync(model.Email);
                if (userCheck != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(userCheck);
                    var role = userRoles.FirstOrDefault();
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Name,userCheck.FirstName+ userCheck.LastName),
                        new Claim(ClaimTypes.NameIdentifier,userCheck.Id),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        claims: authClaims,
                        notBefore: DateTime.UtcNow,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"])), SecurityAlgorithms.HmacSha256)
                        );
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    var response = new ApiResponse()
                    {
                        Status = true,
                        Message = "Login Success !",
                        Data = new LoginResponse
                        {
                            Name = userCheck.FirstName + " " + userCheck.LastName,
                            Role = role,
                            Token = jwtToken,
                            NotBefore = token.ValidFrom,
                            Expiration = token.ValidTo,
                        }
                    };
                    return Ok(response);



                }
            }
            return Ok(new ApiResponse() { Status = false, Message = "Sorry ! Invalid credential.. " });
        }

    }
}
