using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharpYaml.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMS.Security
{
    public class BearerAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BearerAuthHandler> _logger;

        public BearerAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, loggerFactory, encoder, clock)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<BearerAuthHandler>();
        }

        //protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        //{
        //    if (!Request.Headers.ContainsKey("Authorization"))
        //    {
        //        _logger.LogWarning("Missing Authorization Header");
        //        return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        //    }

        //    var token = Request.Headers["Authorization"].ToString();
        //    if (!token.StartsWith("Bearer"))
        //    {
        //        return Task.FromResult(AuthenticateResult.Fail("Bearer token expected"));

        //    }
        //    token = token.Replace("Bearer ","");
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        _logger.LogWarning("Invalid or empty token");
        //        return Task.FromResult(AuthenticateResult.Fail("Invalid or empty token"));
        //    }

        //    try
        //    {
        //        var principal = ValidateToken(token, out var validatedToken);

        //        if (validatedToken is not JwtSecurityToken)
        //        {
        //            _logger.LogWarning("Invalid token format");
        //            return Task.FromResult(AuthenticateResult.Fail("Invalid token format"));
        //        }

        //        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
        //    }
        //    catch (SecurityTokenException ex)
        //    {
        //        return Task.FromResult(AuthenticateResult.Fail("Token validation failed"));
        //    }
        //}

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var endpoint = Context.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                {
                    return AuthenticateResult.NoResult(); 
                }

                var token = Request.Headers["Authorization"].ToString();
                if (!token.StartsWith("Bearer"))
                {
                    await UnauthorizeResponse(Context);
                    return AuthenticateResult.Fail("Bearer token expected");
                }
                token = token.Replace("Bearer ", "");
                var principal = ValidateToken(token, out var validatedToken);



                if (validatedToken is not JwtSecurityToken)
                {
                    await UnauthorizeResponse(Context);

                    return AuthenticateResult.Fail("Token validation failed");
                }

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }
            catch (SecurityTokenException ex)
            {
                await UnauthorizeResponse(Context);

                return AuthenticateResult.Fail("Token validation failed");
            }
        }


        private ClaimsPrincipal ValidateToken(string token, out SecurityToken validatedToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }

        private async Task UnauthorizeResponse(HttpContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Your token is invalid or expired.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#page-6"
            };
            var response = Context.Response;
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(json);
        }
    }
}
