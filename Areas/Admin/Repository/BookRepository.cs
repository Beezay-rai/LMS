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

        public async Task<List<BookGETModel>> GetAllBook()
        {
            return await _context.Book.Where(x => x.Deleted == false).Select(x => new BookGETModel()
            {
                Id=x.Id,
                Name = x.Name,
                AuthorName = x.AuthorName,
                BookCategoryGetList = _context.BookCategory.Where(y=>y.BookId == x.Id).Select(y=> new BookCategoryGetModel()
                {
                    Id=y.Id,
                    BookName=y.Book.Name,
                    CategoryName=y.Category.Name,
                }).ToList(),
               
            }).ToListAsync();
        }
        public async Task<BookModel> GetBookById(int id)
        {
            return await _context.Book.Where(x => x.Id == id && x.Deleted == false).Select(x => new BookModel()
            {
                Id = x.Id,
                Name = x.Name,
                AuthorName = x.AuthorName,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateBook(BookModel model)
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
                        Total=model.Total,
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
            catch (Exception )
            {
                return false;
            }
        }



    }
}
