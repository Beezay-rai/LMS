using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

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
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),

            };

            return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }

        private async Task UnauthorizeResponse(HttpContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",

                Type = "https://datatracker.ietf.org/doc/html/rfc7235#page-6"
            };

            var response = context.Response;
            response.Clear();
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);

            await response.WriteAsync(json);
            await response.Body.FlushAsync();
            await response.CompleteAsync();
        }

    }
}
