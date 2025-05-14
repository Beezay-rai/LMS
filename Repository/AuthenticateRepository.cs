using Google.Apis.Auth;
using LMS.Interfaces;
using LMS.Models;
using LMS.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Identity;

namespace LMS.Repository
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUtility _utility;
        private readonly ITokenService _tokenService;

        public AuthenticateRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUtility utility, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _utility = utility;
            _tokenService = tokenService;
        }



        public async Task<BaseApiResponseModel> Login(LoginModel model)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!signInResult.Succeeded)
            {
                return new ApiResponseModel { Status = false, Message = "Invalid username or password." };
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            var authClaims = await _utility.GetUserClaims(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            return new ApiResponseModel<LoginResponse>
            {
                Status = true,
                Message = "Login successful!",
                Data = new LoginResponse
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    access_token = _tokenService.GenerateAccessToken(authClaims),
                    refresh_token = _tokenService.GenerateRefreshToken(user.Id),
                    role = string.Join(",", userRoles)
                }
            };
        }

        public async Task<BaseApiResponseModel> GoogleLogin(string credentialToken)
        {
            try
            {
                var googleResponse = await GoogleJsonWebSignature.ValidateAsync(credentialToken);
                var user = await _userManager.FindByEmailAsync(googleResponse.Email);

                if (user == null || !googleResponse.EmailVerified)
                {
                    return new ApiResponseModel<object> { Status = false, Message = "User not found or email not verified." };
                }

                var authClaims = await _utility.GetUserClaims(user);

                return new ApiResponseModel<LoginResponse>
                {
                    Status = true,
                    Message = "Login successful!",
                    Data = new LoginResponse
                    {
                        Name = $"{user.FirstName} {user.LastName}",
                        access_token = _tokenService.GenerateAccessToken(authClaims),
                        refresh_token = _tokenService.GenerateRefreshToken(user.Id),

                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<object> { Status = false, Message = $"Google verification failed: {ex.Message}" };
            }
        }

        public async Task<BaseApiResponseModel> GoogleSignUp(string credentialToken)
        {
            try
            {
                var googleResponse = await GoogleJsonWebSignature.ValidateAsync(credentialToken);
                var user = await _userManager.FindByEmailAsync(googleResponse.Email);

                if (user != null)
                {
                    return new ApiResponseModel<object> { Status = false, Message = "User already exists." };
                }

                var newUser = new ApplicationUser
                {
                    FirstName = googleResponse.GivenName,
                    LastName = googleResponse.FamilyName,
                    Email = googleResponse.Email,
                    UserName = googleResponse.Email,
                    Active = true,
                };

                var createUserResult = await _userManager.CreateAsync(newUser);
                return new ApiResponseModel<object>
                {
                    Status = createUserResult.Succeeded,
                    Message = createUserResult.Succeeded ? "User created successfully." : "Error creating user.",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<object> { Status = false, Message = $"Error occurred: {ex.Message}" };
            }
        }

    }
}
