using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Admin.Repository
{
    public class BookRepository : IBook
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookViewModel>> GetAllBook()
        {
            return await _context.Book.Where(x => x.Deleted == false).Select(x => new BookViewModel()
            {
                Name = x.Name,
                AuthorId = x.AuthorId,
                CategoryId = x.CategoryId
            }).ToListAsync();
        }
        public async Task<BookViewModel> GetBookById(int id)
        {
            return await _context.Book.Where(x => x.Id == id && x.Deleted == false).Select(x => new BookViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                AuthorId = x.AuthorId,
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
                        book.AuthorId = model.AuthorId;
                        book.CategoryId = model.CategoryId;
                        book.Deleted= false;    
                        _context.Entry(book).State = EntityState.Modified;
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    Book book = new Book()
                    {
                        Name = model.Name,
                        AuthorId = model.AuthorId,
                        CategoryId = model.CategoryId,
                        Deleted = false
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
                if (Book != null)
                {
                    Book.Deleted = true;
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
