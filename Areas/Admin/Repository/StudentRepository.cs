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
    public class StudentRepository : IStudentRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public StudentRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<BaseApiResponseModel> GetAllStudent()
        {
            var response = new ApiResponseModel<List<StudentModel>>();
            try
            {
                var data = _mapper.Map<List<StudentModel>>(await _context.Student.Where(x => x.delete_status == false).ToListAsync());
                response.Data = data;
                response.Status = true;
                response.HttpStatusCode = HttpStatusCode.OK;
                response.Message = "Available Student List";
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
        public async Task<BaseApiResponseModel> GetStudentById(int id)
        {
            var response = new ApiResponseModel<StudentModel>();
            try
            {
                var data = _mapper.Map<StudentModel>(await _context.Student.Where(x => x.Id == id && !x.delete_status).FirstOrDefaultAsync());
                if (data == null)
                {
                    return new BaseApiResponseModel
                    {
                        Status = false,
                        Message = "Student not found with Id : "+id,
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                response.Data = data;
                response.Status = true;
                response.Message = "Student details retrieved successfully";
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

        public async Task<BaseApiResponseModel> DeleteStudent(int id)
        {
            try
            {
                var student = await _context.Student.FirstOrDefaultAsync(x => x.Id == id);
                if (student == null)
                {
                    return new BaseApiResponseModel
                    {
                        Status = false,
                        Message = "Student not found with Id : " + id,
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                student.delete_status = true;
                student.deleted_date = DateTime.UtcNow;
                _context.Entry(student).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new BaseApiResponseModel
                {
                    Status = true,
                    Message = "Student deleted successfully",
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

        public async Task<BaseApiResponseModel> AddStudent(StudentModel model)
        {
            try
            {
                var student = _mapper.Map<Student>(model);
                student.created_by = _userId;
                student.created_date = DateTime.UtcNow; 
                await _context.Student.AddAsync(student);
                await _context.SaveChangesAsync();
                model = _mapper.Map<StudentModel>(student);
                return new ApiResponseModel<StudentModel>
                {
                    Status = true,
                    Data = model,
                    Message = "Student added successfully",
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

        public async Task<BaseApiResponseModel> UpdateStudent(int studentId, StudentModel model)
        {
            try
            {
                model.Id =studentId;
                var student = await _context.Student.FirstOrDefaultAsync(x => x.Id == studentId && !x.delete_status);
                if (student == null)
                {
                    return new ApiResponseModel<StudentModel>
                    {
                        Status = false,
                        Message = "Student not found",
                        Data = model,
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, student);
                student.updated_date= DateTime.Now;
                student.updated_by = _userId;
                _context.Entry(student).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ApiResponseModel<StudentModel>
                {
                    Status = true,
                    Data = model,
                    Message = "Student updated successfully",
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
