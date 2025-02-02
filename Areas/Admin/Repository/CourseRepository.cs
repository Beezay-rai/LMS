using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class CourseRepository : ICourse
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public CourseRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<CourseModel>> GetAllCourse()
        {
            return await _context.Course.Where(x => x.Deleted == false).Select(x => new CourseModel()
            {
                Id = x.Id,
                Name = x.CourseName,
                Semester = x.Semester,
                Credits = x.Credits,
                Description = x.Description,
            }).ToListAsync();
        }
        public async Task<CourseModel> GetCourseById(int id)
        {
            return await _context.Course.Where(x => x.Id == id && x.Deleted == false).Select(x => new CourseModel()
            {
                Id = x.Id,
                Name = x.CourseName,
                Semester = x.Semester,
                Credits = x.Credits,
                Description = x.Description,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> AddCourse(POSTCourseModel model)
        {
            try
            {
                Course Faculty = new Course()
                {
                    CourseName = model.Name,
                    Credits = model.Credits,
                    Semester = model.Semester,
                    Description = model.Description,
                    Deleted = false,
                    CreatedBy = _userId,
                    CreatedDate = DateTime.UtcNow
                };
                await _context.Course.AddAsync(Faculty);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteCourse(int id)
        {
            try
            {
                var course = await _context.Course.FindAsync(id);
                if (course != null && course.Deleted == false)
                {
                    course.Deleted = true;
                    course.DeletedBy = _userId;
                    course.DeletedDate = DateTime.UtcNow;
                    _context.Entry(course).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCourse(int courseId, POSTCourseModel model)
        {

            try
            {
                var course = await _context.Course.FindAsync(courseId);
                if (course != null && course.Deleted == false)
                {
                    course.CourseName = model.Name;
                    course.Credits = model.Credits;
                    course.Semester = model.Semester;
                    course.Description = model.Description;
                    course.UpdatedBy = _userId;
                    course.UpdatedDate = DateTime.UtcNow;
                    _context.Entry(course).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
