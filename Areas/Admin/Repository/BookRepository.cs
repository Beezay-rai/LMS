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
<<<<<<< HEAD
        private readonly ILogger<BookRepository> _logger;
        private readonly string _userId;

        public BookRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor,ILogger<BookRepository>logger)
=======
        private readonly string _userId;

        public BookRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
<<<<<<< HEAD
            _logger = logger;
        }

        public async Task<List<BookGETModel>> GetAllBook()
        {
            return await _context.Book.Where(x => x.Deleted == false).Select(x => new BookGETModel()
            {
                Id=x.Id,
                BookName = x.BookName,
                AuthorName = x.AuthorName,
                ISBN = x.ISBN,
                PublicationDate = x.PublicationDate,
                Quantity = x.Quantity,  
                BookCategoryDetailList = _context.BookCategoryDetail.Where(z=>z.BookId == x.Id).Select(z=> new BookCategoryDetailGETModel()
                {
                    Id=z.Id,
                    CategoryId = z.CategoryId,
                    CategoryName = _context.Category.Where(y=>y.Id == z.CategoryId && y.IsDeleted == false).Select(y=>y.Name).FirstOrDefault()

                }).ToList()
            }).ToListAsync();
        }
        public async Task<BookGETModel> GetBookById(int id)
        {
            return await _context.Book.Where(x => x.Id == id && x.Deleted == false).Select(x => new BookGETModel()
            {

                Id = x.Id,
                BookName = x.BookName,
                AuthorName = x.AuthorName,
                ISBN = x.ISBN,
                PublicationDate = x.PublicationDate,
                Quantity = x.Quantity,
                BookCategoryDetailList = _context.BookCategoryDetail.Where(z => z.BookId == x.Id).Select(z => new BookCategoryDetailGETModel()
                {
                    Id=z.Id,
                    CategoryId = z.CategoryId,
                    CategoryName = _context.Category.Where(y => y.Id == z.CategoryId && y.IsDeleted == false).Select(y => y.Name).FirstOrDefault()

                }).ToList()
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateBook(BookModel model)
        {
            using(var transaction  = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model.Id > 0)
                    {
                        var book = await _context.Book.FindAsync(model.Id);
                        if (book != null)
                        {
                            book.BookName = model.BookName;
                            book.AuthorName = model.AuthorName;
                            book.ISBN = model.ISBN;
                            book.PublicationDate = model.PublicationDate;
                            book.Quantity = model.Quantity;
                            book.UpdatedBy = _userId;
                            book.UpdatedDate = DateTime.UtcNow;
                            _context.Entry(book).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            if (model.BookCategoryDetailList.Count > 0)
                            {
                                foreach(var item in model.BookCategoryDetailList)
                                {
                                    var bookCategoryDetail = await _context.BookCategoryDetail.Where(x => x.BookId == book.Id && x.Id == item.Id).FirstOrDefaultAsync();
                                    if(bookCategoryDetail != null)
                                    {
                                        bookCategoryDetail.BookId = item.BookId;
                                        bookCategoryDetail.CategoryId = item.CategoryId;
                                        _context.Entry(bookCategoryDetail).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }


                        }
                    }
                    else
                    {
                        Book book = new Book()
                        {
                            BookName = model.BookName,
                            AuthorName = model.AuthorName,
                            ISBN = model.ISBN,
                            PublicationDate = model.PublicationDate,
                            Quantity = model.Quantity,
                            Deleted = false,
                            CreatedBy = _userId,
                            CreatedDate = DateTime.UtcNow
                        };
                        await _context.Book.AddAsync(book);
                        await _context.SaveChangesAsync();


                        if (model.BookCategoryDetailList.Count > 0)
                        {
                            foreach (var item in model.BookCategoryDetailList)
                            {
                                BookCategoryDetail bookCategoryDetail = new BookCategoryDetail()
                                {
                                    BookId = book.Id,
                                    CategoryId = item.CategoryId,
                                };
                                await _context.BookCategoryDetail.AddAsync(bookCategoryDetail);
                                await _context.SaveChangesAsync();
                            }

                        }
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error , DateTime:{DateTime.UtcNow}, UserId:{_userId},Error Description:{ex} ");
                    await transaction.RollbackAsync();

                    return false;
                }

            }

=======
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
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
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
<<<<<<< HEAD
                _logger.LogInformation($"Error , DateTime:{DateTime.UtcNow}, UserId:{_userId},Error Description:{ex} ");
=======
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
                return false;
            }
        }



    }
}
