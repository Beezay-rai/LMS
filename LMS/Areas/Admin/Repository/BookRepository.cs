using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Admin.Repository
{
    public class BookRepository : IBook
    {
        private readonly ApplicationDbContext _context;

        public BookRepository( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBook(BookViewModel model)
        {
            try
            {
                Book book = new Book()
                {
                    Name = model.Name,
                    AuthorId = model.AuthorId,
                    CategoryId = model.CategoryId
                };
                await _context.Book.AddAsync(book);
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
                var book = await _context.Book.FindAsync(id);
                if (book != null)
                {
                    _context.Book.Remove(book);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditBook(BookViewModel model)
        {
            try
            {
                var book = await _context.Book.FindAsync(model.Id);
                if (book != null)
                {
                    book.Name = model.Name;
                    book.AuthorId = model.AuthorId;
                    book.CategoryId = model.CategoryId;
                    _context.Entry(book).State = EntityState.Modified;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public async Task<List<BookViewModel>> GetAllBook()
        {
            return await _context.Book.Select(x => new BookViewModel()
            {
                Name = x.Name,
                AuthorId = x.AuthorId,
                CategoryId = x.CategoryId
            }).ToListAsync();
        }

        public async Task<BookViewModel> GetBookById(int id)
        {
            return await _context.Book.Where(x => x.Id == id).Select(x => new BookViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                AuthorId = x.AuthorId,
                CategoryId = x.CategoryId
            }).FirstOrDefaultAsync() ?? new BookViewModel();
        }
    }
}
