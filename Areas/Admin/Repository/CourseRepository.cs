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
    public class CourseRepository : ICourseRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<CourseRepository> _logger;
        private readonly string _userId;

        public CourseRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper, ILogger<CourseRepository> logger)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<BaseApiResponseModel> GetAllCourse()
        {
            try
            {
                var courses = await _context.Course
                    .Where(x => !x.Deleted)
                    .ToListAsync();

                var data = _mapper.Map<List<CourseModel>>(courses);


                return new ApiResponseModel<List<CourseModel>>
                {
                    Status = true,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Course List Retrieved",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                        new ErrorDetailModel { Message = ex.InnerException?.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        // Get Course By Id
        public async Task<BaseApiResponseModel> GetCourseById(int id)
        {
            try
            {
                var course = await _context.Course
                    .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);

                if (course == null)
                {
                    return new ApiResponseModel<int>
                    {
                        Status = false,
                        Message = "Course not found with Id : "+id,
                        Data = id,
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                var data = _mapper.Map<CourseModel>(course);



                return new ApiResponseModel<CourseModel>
                {
                    Status = true,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Course retrieved successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                        new ErrorDetailModel { Message = ex.InnerException?.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        // Add a New Course
        public async Task<BaseApiResponseModel> AddCourse(CourseModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var course = _mapper.Map<Course>(model);
                    course.CreatedBy = _userId;
                    course.CreatedDate = DateTime.UtcNow;
                    course.Deleted = false;

                    await _context.Course.AddAsync(course);
                    await _context.SaveChangesAsync();


                    var data = _mapper.Map<CourseModel>(course);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResponseModel<CourseModel>
                    {
                        Status = true,
                        HttpStatusCode = HttpStatusCode.Created,
                        Message = "Course added successfully",
                        Data = data
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiErrorResponseModel<ErrorDetailModel>
                    {
                        Status = false,
                        Message = ex.Message,
                        Errors = new List<ErrorDetailModel>
                        {
                            new ErrorDetailModel { Message = ex.InnerException?.Message, StackTrace = ex.StackTrace }
                        },
                        HttpStatusCode = HttpStatusCode.InternalServerError
                    };
                }
            }
        }

        // Update Course
        public async Task<BaseApiResponseModel> UpdateCourse(int courseId, CourseModel model)
        {
            try
            {
                model.Id = courseId;
                var course = await _context.Course.FirstOrDefaultAsync(x => x.Id == courseId && !x.Deleted);

                if (course == null)
                {
                    return new ApiResponseModel<CourseModel>
                    {
                        Status = false,
                        Message = "Course not found",
                        Data =model,
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, course);
                course.UpdatedBy = _userId;
                course.UpdatedDate = DateTime.UtcNow;

                _context.Entry(course).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ApiResponseModel<CourseModel>
                {
                    Status = true,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Course updated successfully",
                    Data = _mapper.Map<CourseModel>(course)
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                        new ErrorDetailModel { Message = ex.InnerException?.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        // Delete Course
        public async Task<BaseApiResponseModel> DeleteCourse(int id)
        {
            try
            {
                var course = await _context.Course.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
                if (course == null)
                {
                    return new ApiErrorResponseModel<bool>
                    {
                        Status = false,
                        Message = "Course not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                course.Deleted = true;
                course.DeletedBy = _userId;
                course.DeletedDate = DateTime.UtcNow;

                _context.Entry(course).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new BaseApiResponseModel
                {
                    Status = true,
                    Message = "Course deleted successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                        new ErrorDetailModel { Message = ex.InnerException?.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
