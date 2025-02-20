using AutoMapper;
using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using LMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public CategoryRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<BaseApiResponseModel> GetAllCategory()
        {
            var response = new ApiResponseModel<List<CategoryModel>>();
            try
            {
                var data = _mapper.Map<List<CategoryModel>>(await _context.Category.Where(x => x.delete_status == false).ToListAsync());
                response.Data = data;
                response.Status = true;
                response.HttpStatusCode = HttpStatusCode.OK;
                response.Message = "Available Category List";
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiErrorResponseModel<ErrorDetailModel>()
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };

                return errorResponse;

            }

        }
        public async Task<BaseApiResponseModel> GetCategoryById(int id)
        {
            var response = new ApiResponseModel<CategoryModel>();
            try
            {
                var data = _mapper.Map<CategoryModel>(await _context.Category.Where(x => x.Id == id && !x.delete_status).FirstOrDefaultAsync());
                if (data == null)
                {
                    return new ApiErrorResponseModel<CategoryModel>
                    {
                        Status = false,
                        Message = "Category not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                response.Data = data;
                response.Status = true;
                response.Message = "Category details retrieved successfully";
                response.HttpStatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };
            }
        }

        public async Task<BaseApiResponseModel> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return new ApiErrorResponseModel<bool>
                    {
                        Status = false,
                        Message = "Category not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                category.delete_status = true;
               
                await _context.SaveChangesAsync();

                return new BaseApiResponseModel
                {
                    Status = true,
                    Message = "Category deleted successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };
            }
        }

        public async Task<BaseApiResponseModel> AddCategory(CategoryModel model)
        {
            try
            {
                var category = _mapper.Map<Category>(model);

                await _context.Category.AddAsync(category);
                await _context.SaveChangesAsync();
                model = _mapper.Map<CategoryModel>(category);
                return new ApiResponseModel<CategoryModel>
                {
                    Status = true,
                    Data = model,
                    Message = "Category added successfully",
                    HttpStatusCode = HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };
            }
        }

        public async Task<BaseApiResponseModel> UpdateCategory(int categoryId, CategoryModel model)
        {
            try
            {
                model.Id = categoryId;
                var category = await _context.Category.FirstOrDefaultAsync(x => x.Id == categoryId && !x.delete_status);
                if (category == null)
                {
                    return new ApiResponseModel<CategoryModel>
                    {
                        Status = false,
                        Message = "Category not found",
                        Data = model,
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                category.Name = model.Name;
                await _context.SaveChangesAsync();

                return new ApiResponseModel<CategoryModel>
                {
                    Status = true,
                    Data = model,
                    Message = "Category updated successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };
            }
        }

    }
}
