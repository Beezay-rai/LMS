using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class IssueBookRepository : IIssueBook
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private string _userId;
        public IssueBookRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<List<IssueBookGETModel>> GetAllIssueBook()
        {
            return await _context.IssueBook.Where(x => x.Deleted == false).Select(x => new IssueBookGETModel
            {
                Id = x.Id,
                BookId = x.BookId,
                BookName = x.Book.Name,
                IssuedDate = x.CreatedDate,
                ReturnDate = x.ReturnDate,
                Remarks = x.Remarks,
                ReturnStatus = x.ReturnStatus,
                StudentId = x.StudentId,
                //StudentFullName = x.Student.FirstName + " " + x.Student.LastName,

            }).ToListAsync();
        }

        public async Task<IssueBookGETModel> GetIssueBookById(int id)
        {
            return await _context.IssueBook.Where(x => x.Id == id && x.Deleted == false).Select(x => new IssueBookGETModel()
            {
                Id = x.Id,
                BookId = x.BookId,
                BookName = x.Book.Name,
                IssuedDate = x.CreatedDate,
                ReturnDate = x.ReturnDate,
                Remarks = x.Remarks,
                ReturnStatus = x.ReturnStatus,
                StudentId = x.StudentId,
                //StudentFullName = x.Student.FirstName + " " + x.Student.LastName,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateIssueBook(IssueBookModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var IssueBook = await _context.IssueBook.FindAsync(model.Id);
                    if (IssueBook != null)
                    {
                        IssueBook.BookId = model.BookId;
                        IssueBook.ReturnDate = model.ReturnDate;
                        IssueBook.Remarks = model.Remarks;
                        IssueBook.StudentId = model.StudentId;
                        IssueBook.ReturnStatus = false;
                        IssueBook.UpdatedBy = _userId;
                        IssueBook.UpdatedDate = DateTime.UtcNow;
                        IssueBook.Deleted = false;
                        _context.Entry(IssueBook).State = EntityState.Modified;
                    }
                    else { return false; }
                }
                else
                {
                    IssueBook IssueBook = new IssueBook()
                    {
                        Id = model.Id,
                        BookId = model.BookId,
                        ReturnDate = model.ReturnDate,
                        Remarks = model.Remarks,
                        ReturnStatus = false,
                        StudentId = model.StudentId,
                        CreatedBy = _userId,
                        CreatedDate = DateTime.UtcNow,
                        Deleted = false
                    };
                    await _context.IssueBook.AddAsync(IssueBook);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteIssueBook(int id)
        {
            try
            {
                var IssueBook = await _context.IssueBook.FindAsync(id);
                if (IssueBook != null)
                {
                    IssueBook.Deleted = true;
                    IssueBook.DeletedDate = DateTime.UtcNow;
                    IssueBook.DeletedBy = _userId;
                    _context.Entry(IssueBook).State = EntityState.Modified;
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

        public async Task<bool> ReturnedIssuedBook(int id, bool status)
        {
            try
            {
                var issuedBook = await _context.IssueBook.FindAsync(id);
                if (issuedBook != null && issuedBook.Deleted==false)
                {
                    issuedBook.ReturnStatus = status;
                    issuedBook.ReturnDate = DateTime.UtcNow;
                    _context.Entry(issuedBook).State = EntityState.Modified;
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
