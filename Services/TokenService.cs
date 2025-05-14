using LMS.Utility;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken(string userId);
        Task<string> GenerateAccessTokenFromRefreshToken(string refreshToken);
        (bool valid, string userId) ValidateRefreshToken(string refreshToken);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUtility _utility;
        private readonly SigningCredentials _signingCredentials;
        private readonly SymmetricSecurityKey _secret;
        private readonly int _refreshTokenExpire;
        private readonly int _accessTokenExpire;
        public TokenService(IConfiguration configuration, IUtility utility)
        {
            _configuration = configuration;
            _utility = utility;
            _secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _signingCredentials = new SigningCredentials(_secret, SecurityAlgorithms.HmacSha256);
            _accessTokenExpire = int.Parse(_configuration["JWT:AccessTokenExpireMin"]);
            _refreshTokenExpire = int.Parse(_configuration["JWT:RefreshTokenExpireMin"]);
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims) =>
            GenerateToken(claims.Append(new Claim("token_type", "access_token")),
                          _accessTokenExpire);



        public string GenerateRefreshToken(string userId) =>
            GenerateToken(new List<Claim>
            {
                new Claim("token_type", "refresh_token"),
                new Claim("user_id", userId)
            }, _refreshTokenExpire);


        public async Task<string> GenerateAccessTokenFromRefreshToken(string refreshToken)
        {
            var (valid, userId) = ValidateRefreshToken(refreshToken);
            if (!valid) return string.Empty;

            var user = await _utility.GetUserById(userId);
            if (user == null) return string.Empty;

            var claims = await _utility.GetUserClaims(user);
            return claims != null ? GenerateAccessToken(claims) : string.Empty;
        }

        public (bool valid, string userId) ValidateRefreshToken(string refreshToken)
        {
            var principal = GetClaimsFromToken(refreshToken);
            if (principal == null) return (false, null);

            var tokenType = ExtractClaim(principal, "token_type");
            var userId = ExtractClaim(principal, "user_id");

            return (tokenType == "refresh_token" && !string.IsNullOrWhiteSpace(userId), userId);
        }

        private string GenerateToken(IEnumerable<Claim> claims, int expireMinutes)
        {
            var todayDate = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsPrincipal GetClaimsFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = _secret,
                }, out _);
            }
            catch
            {
                return null;
            }
        }

        private string ExtractClaim(ClaimsPrincipal principal, string claimType) =>
            principal.FindFirst(claimType)?.Value;
    }
}
