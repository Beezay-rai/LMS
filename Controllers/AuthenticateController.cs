using LMS.Interface;
using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateRepository _repo;
        private readonly ITokenService _tokenService;
        public AuthenticateController(IAuthenticateRepository repo,ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }
 
        [HttpPost]
        [Route("api/v1/users")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _repo.SignUp(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/v1/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _repo.Login(model));
        }

        [HttpPost]
        [Route("api/v1/auth/google")]
        public async Task<IActionResult> LoginFromGoogle(string token)
        {
            return Ok(await _repo.GoogleLogin(token));
        }

        [HttpPost]
        [Route("api/v1/auth/refresh/{refresh_token}")]
        public async Task<IActionResult> RefreshToken(string refresh_token)
        {
            var (valid,user_id) =_tokenService.ValidateRefreshToken(refresh_token);
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
                    Status=false,
                    Message="Invalid Refresh Token !"
                });
            }
        }


    }
}
