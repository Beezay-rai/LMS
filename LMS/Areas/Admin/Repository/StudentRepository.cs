using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;

namespace LMS.Areas.Admin.Repository
{
    public class StudentRepository : IStudent
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateStudent(StudentViewModel model)
        {
            try
            {
                Student student = new Student()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    GenderId = model.GenderId
                };
                await _context.Student.AddAsync(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> DeleteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditStudent(StudentViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentViewModel>> GetAllStudent()
        {
            throw new NotImplementedException();
        }

        public Task<StudentViewModel> GetStudentById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
