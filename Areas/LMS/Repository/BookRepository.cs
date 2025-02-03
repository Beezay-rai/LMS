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
    public class BookRepository : IBook
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<BookRepository> _logger;
        private readonly string _userId;

        public BookRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper, ILogger<BookRepository> logger)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<BaseApiResponseModel> GetAllBook()
        {
            try
            {
                var books = await _context.Book
                    .Where(x => !x.delete_status)
                    .ToListAsync();

                var data = _mapper.Map<List<BookModel>>(books);

                return new ApiResponseModel<List<BookModel>>
                {
                    Status = true,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Book List Retrieved",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseApiResponseModel> GetBookById(int id)
        {
            try
            {
                var book = await _context.Book
                    .FirstOrDefaultAsync(x => x.id == id && !x.delete_status);

                if (book == null)
                {
                    return new ApiErrorResponseModel<BookModel>
                    {
                        Status = false,
                        Message = "Book not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                var data = _mapper.Map<BookModel>(book);

                return new ApiResponseModel<BookModel>
                {
                    Status = true,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Book retrieved successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseApiResponseModel> AddBook(BookModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var book = _mapper.Map<Book>(model);
                    book.created_by = _userId;
                    book.created_date = DateTime.UtcNow;
                    book.delete_status = false;

                    await _context.Book.AddAsync(book);
                    await _context.SaveChangesAsync();
                    if (book.id > 0 && model.book_categories?.Any() == true)
                    {
                        var bookCategoryDetails = model.book_categories
                            .Select(categoryId => new BookCategoryDetail
                            {
                                BookId = book.id,
                                CategoryId = categoryId
                            }).ToList();

                        await _context.BookCategoryDetail.AddRangeAsync(bookCategoryDetails);
                    }
                    var data = _mapper.Map<BookModel>(book);
                    data.book_categories = model.book_categories;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ApiResponseModel<BookModel>
                    {
                        Status = true,
                        Message = "Book added successfully",
                        HttpStatusCode = HttpStatusCode.Created,
                        Data = data
                    };


                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiErrorResponseModel<ErrorDetailModel>
                    {
                        Status = false,
                        Message = ex.Message,
                        Errors = new List<ErrorDetailModel>
                        {
                             new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                        },
                        HttpStatusCode = HttpStatusCode.InternalServerError
                    };
                }

            }
        }

        public async Task<BaseApiResponseModel> UpdateBook(int bookId, BookModel model)
        {
            try
            {
                model.Id = bookId;
                var book = await _context.Book.FirstOrDefaultAsync(x => x.id == bookId && !x.delete_status);
                if (book == null)
                {
                    return new ApiErrorResponseModel<BookModel>
                    {
                        Status = false,
                        Message = "Book not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, book);
                book.updated_by = _userId;
                book.updated_date = DateTime.UtcNow;

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new ApiResponseModel<BookModel>
                {
                    Status = true,
                    Message = "Book updated successfully",
                    HttpStatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<BookModel>(book)
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseApiResponseModel> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Book.FirstOrDefaultAsync(x => x.id == id && !x.delete_status);
                if (book == null)
                {
                    return new ApiResponseModel<int>
                    {
                        Status = false,
                        Data = id,
                        Message = "Book not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                book.delete_status = true;
                book.deleted_by = _userId;
                book.deleted_date = DateTime.UtcNow;

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new BaseApiResponseModel
                {
                    Status = true,
                    Message = "Book deleted successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<Exception>
                {
                    Status = false,
                    Message = ex.Message,
                    Errors = new List<Exception> { ex },
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
