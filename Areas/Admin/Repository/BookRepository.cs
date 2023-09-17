using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class BookRepository : IBook
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public BookRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<BookGETViewModel>> GetAllBook()
        {
            return await _context.Book.Where(x => x.Deleted == false).Select(x => new BookGETViewModel()
            {
                Id=x.Id,
                Name = x.Name,
                AuthorName = x.AuthorName,
                CategoryId = x.CategoryId,
                CategoryName=x.Category.Name,
            }).ToListAsync();
        }
        public async Task<BookViewModel> GetBookById(int id)
        {
            return await _context.Book.Where(x => x.Id == id && x.Deleted == false).Select(x => new BookViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                AuthorName = x.AuthorName,
                CategoryId = x.CategoryId
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateBook(BookViewModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var book = await _context.Book.FindAsync(model.Id);
                    if (book != null)
                    {
                        book.Name = model.Name;
                        book.AuthorName = model.AuthorName;
                        book.CategoryId = model.CategoryId;
                        book.Deleted= false;
                        book.UpdatedBy = _userId;
                        book.UpdatedDate= DateTime.UtcNow;
                        _context.Entry(book).State = EntityState.Modified;
                    }
                    else { return false; }
                }
                else
                {
                    Book book = new Book()
                    {
                        Name = model.Name,
                        AuthorName = model.AuthorName,
                        CategoryId = model.CategoryId,
                        Deleted = false,
                        CreatedBy=_userId,
                        CreatedDate= DateTime.UtcNow
                    };
                    await _context.Book.AddAsync(book);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteBook(int id)
        {
            try
            {
                var Book = await _context.Book.FindAsync(id);
                if (Book != null && Book.Deleted==false)
                {
                    Book.Deleted = true;
                    Book.DeletedBy=_userId;
                    Book.DeletedDate= DateTime.UtcNow;
                    _context.Entry(Book).State = EntityState.Modified;
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
