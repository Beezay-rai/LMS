using Google.Apis.Auth;
using LMS.Crypto;
using LMS.Data;
using LMS.Interface;
using LMS.Models;
using LMS.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Repository
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUtility _utility;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AuthenticateRepository(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IUtility utility,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _utility = utility;
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<BaseApiResponseModel> SignUp(SignUpModel model)
        {
            var response = new ApiResponseModel<object>();
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new ApiResponseModel<object> { Status = false, Message = "User already exists!" };
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newUser = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                };

                var createUserResult = await _userManager.CreateAsync(newUser, model.Password);
                if (!createUserResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponseModel<object>
                    {
                        Status = false,
                        Message = "Error creating user.",
                        Data = createUserResult.Errors.Select(e => e.Description)
                    };
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, model.Role);
                if (!addToRoleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponseModel<object>
                    {
                        Status = false,
                        Message = "Error assigning role.",
                        Data = addToRoleResult.Errors.Select(e => e.Description)
                    };
                }

                await transaction.CommitAsync();
                return new ApiResponseModel<object> { Status = true, Message = "User created successfully!" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponseModel<object> { Status = false, Message = ex.Message };
            }
        }

        public async Task<BaseApiResponseModel> Login(LoginModel model)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!signInResult.Succeeded)
            {
                return new ApiResponseModel<object> { Status = false, Message = "Invalid username or password." };
            }

            var user = await _userManager.FindByNameAsync(model.Username);
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
                    NotBefore = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(1),
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
                        NotBefore = DateTime.UtcNow,
                        Expiration = DateTime.UtcNow.AddDays(1),
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
