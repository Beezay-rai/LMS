using Google.Apis.Auth;
using LMS.Data;
using LMS.Interface;
using LMS.Models;
using LMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
        private readonly IUtility _utility;
        private readonly ApplicationDbContext _context;
        public AccountRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IUtility utility)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _utility = utility;
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
                    var check = await _userManager.FindByEmailAsync(newUser.Email);
                    if (check == null)
                    {
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
                            response.Data = createUser.Errors.Select(x => x.Description).Concat(addToRole.Errors.Select(x => x.Description));
                            response.Message = "Error in creating new user !";
                            await transaction.RollbackAsync();

                        }

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



        public async Task<ApiResponse> Login(LoginModel model)
        {
            var user = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (user.Succeeded)
            {
                
                var userCheck = await _userManager.FindByNameAsync(model.Username);
                if (userCheck != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(userCheck);
                    var role = userRoles.FirstOrDefault() ?? "";
                    var authClaims = await _utility.GetUserClaims(userCheck);
                    var response = new ApiResponse()
                    {
                        Status = true,
                        Message = "Login Success !",
                        Data = new LoginResponse
                        {
                            Name = userCheck.FirstName + " " + userCheck.LastName,
                            Role = role,
                            Token = _utility.GenerateJWTToken(authClaims),
                            NotBefore = DateTime.UtcNow,
                            Expiration = DateTime.UtcNow.AddDays(1),
                        }
                    };
                    return response;



                }
            }
            return new ApiResponse() { Status = false };
        }

        public async Task<ApiResponse> GoogleLogin(string crediantialToken)
        {
            var responsemodel = new ApiResponse();
            try
            {
                var Googleresponse = await GoogleJsonWebSignature.ValidateAsync(crediantialToken);
                var userCheck = await _userManager.FindByEmailAsync(Googleresponse.Email);

                if (userCheck != null && Googleresponse.EmailVerified)
                {
                    var authClaims = await _utility.GetUserClaims(userCheck);
                    var userRoles = await _userManager.GetRolesAsync(userCheck);
                    var role = userRoles.FirstOrDefault() ?? " ";
                    responsemodel = new ApiResponse()
                    {
                        Status = true,
                        Message = "Login Success !",
                        Data = new LoginResponse
                        {
                            Name = userCheck.FirstName + " " + userCheck.LastName,
                            Role = role,
                            Token = _utility.GenerateJWTToken(authClaims),
                            NotBefore = DateTime.UtcNow,
                            Expiration = DateTime.UtcNow.AddDays(1),
                        }
                    };
                }
                else
                {
                    responsemodel.Status = false;
                    responsemodel.Message = "User not found ! T_T";

                }
            }
            catch (Exception ex)
            {

                return responsemodel = new ApiResponse() { Status = false, Message = $"Google Verification Failed ! , Error Message: {ex.Message} " };
            }

            return responsemodel;


        }

        public async Task<ApiResponse> GoogleSignUp(string crediantialToken)
        {
            var responsemodel = new ApiResponse();

            try
            {
                var Googleresponse = await GoogleJsonWebSignature.ValidateAsync(crediantialToken);
                var userCheck = await _userManager.FindByEmailAsync(Googleresponse.Email);

                if (userCheck == null && Googleresponse.EmailVerified)
                {

                    var newuser = new ApplicationUser()
                    {
                        FirstName = Googleresponse.GivenName,
                        LastName = Googleresponse.FamilyName,
                        Email = Googleresponse.Email,
                        UserName = Googleresponse.Email,
                        Active = true,
                    };
                    var createUser = await _userManager.CreateAsync(newuser);

                    responsemodel.Status = createUser.Succeeded;
                    responsemodel.Message = createUser.Succeeded ? "Success in creating user  :D" : "Error in creating User T_T";
                }
                else
                {
                    responsemodel.Status = false;
                    responsemodel.Message = "User exists already ! :D";
                }
            }
            catch (Exception ex)
            {
                responsemodel.Status = false;
                responsemodel.Message = $"Error Occured  ! ,  Error Message :{ex.Message}";
            }


            return responsemodel;

        }
    }
}
