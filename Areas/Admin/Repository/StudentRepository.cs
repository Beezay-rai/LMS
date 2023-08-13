using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Admin.Repository
{
    public class StudentRepository : IStudent
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<StudentViewModel>> GetAllStudent()
        {
            return await _context.Student.Where(x => x.Deleted == false).Select(x => new StudentViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                GenderId = x.GenderId,
            }).ToListAsync();
        }
        public async Task<StudentViewModel> GetStudentById(int id)
        {
            return await _context.Student.Where(x => x.Id == id && x.Deleted == false).Select(x => new StudentViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                GenderId = x.GenderId,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateStudent(StudentViewModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var student = await _context.Student.Where(x => x.Id == model.Id && x.Deleted == false).FirstOrDefaultAsync();
                    if (student != null)
                    {
                        student.FirstName = model.FirstName;
                        student.LastName = model.LastName;
                        student.BirthDate = model.BirthDate;
                        student.GenderId = model.GenderId;
                        _context.Entry(student).State = EntityState.Modified;
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    Student student = new Student()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                        GenderId = model.GenderId,
                        Deleted = false
                    };
                    await _context.Student.AddAsync(student);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteStudent(int id)
        {
            var data = await _context.Student.Where(x => x.Id == id && x.Deleted == false).FirstOrDefaultAsync();
            if (data != null)
            {
                data.Deleted = true;
                _context.Entry(data).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;

            }
            else { return false; }
        }

    }
}
