using LMS.Interfaces;
using LMS.Models;
using LMS.Models.Settings;
using LMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LMS.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateRepository _repo;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public AuthenticateController(IAuthenticateRepository repo, ITokenService tokenService, IEmailService emailService)
        {
            _repo = repo;
            _tokenService = tokenService;
            _emailService = emailService;    
        }



        [HttpPost]
        [Route("api/v1/auth/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _repo.Login(model));
        }

        [HttpPost]
        [Route("api/v1/auth/google")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginFromGoogle(string token)
        {
            return Ok(await _repo.GoogleLogin(token));
        }

        [HttpPost]
        [Route("api/v1/auth/refresh/{refresh_token}")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string refresh_token)
        {
            var (valid, user_id) = _tokenService.ValidateRefreshToken(refresh_token);
            if (valid)
            {
                return Ok(new
                {
                    refresh_token = _tokenService.GenerateRefreshToken(user_id),
                    access_token = await _tokenService.GenerateAccessTokenFromRefreshToken(refresh_token)
                });
            }
            else
            {
                return BadRequest(new BaseApiResponseModel()
                {
                    Status = false,
                    Message = "Invalid Refresh Token !"
                });
            }
        }


    }
}
