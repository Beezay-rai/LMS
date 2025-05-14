using LMS.Areas.Admin.Interface;
using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LMS.Areas.Admin.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<BaseApiResponseModel> GetAllUser()
        {
            try
            {
                var response = new ApiResponseModel<List<UserModel>>()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "List of user",
                    Data = await _userManager.Users.Select(x => new UserModel()
                    {
                        Active = x.Active,
                        Email = x.Email,
                        Name = x.FirstName + " " + x.LastName,
                    }).ToListAsync(),
                };
                return response;

            }
            catch (Exception ex)
            {
                var error = new BaseApiResponseModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Status = false,
                    Message = ex.Message,
                };
                return error;
            }
        }

        public async Task<BaseApiResponseModel> SignUpUser(SignUpModel model)
        {
            var response = new ApiResponseModel();
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                response.Status = false;
                response.Message = "User already exists!";
                return response;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var createUserResult = await _userManager.CreateAsync(newUser, model.Password);
                if (!createUserResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponseModel<object>
                    {
                        HttpStatusCode =HttpStatusCode.InternalServerError,
                        Status = false,
                        Message = "Error creating user.",
                        Data = createUserResult.Errors.Select(e => e.Description)
                    };
                }

                //var addToRoleResult = await _userManager.AddToRoleAsync(newUser, model.Role);
                //if (!addToRoleResult.Succeeded)
                //{
                //    await transaction.RollbackAsync();
                //    return new ApiResponseModel<object>
                //    {
                //        Status = false,
                //        Message = "Error assigning role.",
                //        Data = addToRoleResult.Errors.Select(e => e.Description)
                //    };
                //}

                await transaction.CommitAsync();
                return new ApiResponseModel {HttpStatusCode =HttpStatusCode.OK,  Status = true, Message = "User created successfully!" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponseModel { HttpStatusCode = HttpStatusCode.InternalServerError, Status = false, Message = ex.Message };
            }
        }
    }
}
