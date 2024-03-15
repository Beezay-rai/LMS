﻿
using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Utility
{
    public class Utilities : IUtility
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public Utilities(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }
        public string GenerateJWTToken(List<Claim> authClaims)
        {
            var token = new JwtSecurityToken(
                       issuer: _configuration["JWT:ValidIssuer"],
                       audience: _configuration["JWT:ValidAudience"],
                       claims: authClaims,
                       notBefore: DateTime.UtcNow,
                       expires: DateTime.UtcNow.AddDays(1),
                       signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"])), SecurityAlgorithms.HmacSha256)
                       );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public async Task<List<Claim>> GetUserClaims(ApplicationUser user)
        {

            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault() ?? " ";
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Name,user.FirstName+ user.LastName),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };
            return authClaims;
        }
    }
}
