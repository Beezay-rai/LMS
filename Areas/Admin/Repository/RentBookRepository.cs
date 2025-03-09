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
    public class RentBookRepository : IRentBookRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public RentBookRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<BaseApiResponseModel> GetAllRentBook()
        {
            var response = new ApiResponseModel<List<RentBookModel>>();
            try
            {
                var data = _mapper.Map<List<RentBookModel>>(await _context.RentBook.Where(x => x.deleted == false).ToListAsync());
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        var rentBookDetail = _mapper.Map<List<RentBookDetailModel>>(await _context.RentBookDetail.Where(x => x.rent_book_id == item.Id).ToListAsync());
                        item.rent_book = rentBookDetail;
                    }

                }
                response.Data = data;
                response.Status = true;
                response.HttpStatusCode = HttpStatusCode.OK;
                response.Message = "Available RentBook List";
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiErrorResponseModel<ErrorDetailModel>()
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };

                return errorResponse;

            }

        }
        public async Task<BaseApiResponseModel> GetRentBookById(int id)
        {
            var response = new ApiResponseModel<RentBookModel>();
            try
            {
                var data = _mapper.Map<RentBookModel>(await _context.RentBook.Where(x => x.id == id && !x.deleted).FirstOrDefaultAsync());
                if (data != null)
                {
                    data.rent_book = _mapper.Map<List<RentBookDetailModel>>(await _context.RentBookDetail.Where(x => x.rent_book_id == data.Id).ToListAsync());
                }
                if (data == null)
                {
                    return new ApiErrorResponseModel<RentBookModel>
                    {
                        Status = false,
                        Message = "RentBook not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                response.Data = data;
                response.Status = true;
                response.Message = "RentBook details retrieved successfully";
                response.HttpStatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };
            }
        }

        public async Task<BaseApiResponseModel> DeleteRentBook(int id)
        {
            try
            {
                var RentBook = await _context.RentBook.FirstOrDefaultAsync(x => x.id == id);
                if (RentBook == null)
                {
                    return new ApiErrorResponseModel<bool>
                    {
                        Status = false,
                        Message = "RentBook not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                RentBook.deleted = true;
                _context.Entry(RentBook).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new BaseApiResponseModel
                {
                    Status = true,
                    Message = "RentBook deleted successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResponseModel<ErrorDetailModel>
                {
                    Status = false,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                };
            }
        }

        public async Task<BaseApiResponseModel> AddRentBook(RentBookModel model)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var RentBook = _mapper.Map<RentBook>(model);
                    await _context.RentBook.AddAsync(RentBook);
                    await _context.SaveChangesAsync();

                    var rentBookDetails = _mapper.Map<List<RentBookDetail>>(model.rent_book);
                    foreach (var detail in rentBookDetails)
                    {
                        detail.rent_book_id = RentBook.id;
                    }
                    await _context.RentBookDetail.AddRangeAsync(rentBookDetails);
                    await _context.SaveChangesAsync();


                    model = _mapper.Map<RentBookModel>(RentBook);
                    model.rent_book = _mapper.Map<List<RentBookDetailModel>>(rentBookDetails);
                    await tran.CommitAsync();
                    return new ApiResponseModel<RentBookModel>
                    {
                        Status = true,
                        Data = model,
                        Message = "RentBook added successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    return new ApiErrorResponseModel<ErrorDetailModel>
                    {
                        Status = false,
                        Message = ex.Message,
                        HttpStatusCode = HttpStatusCode.InternalServerError,
                        Errors = new List<ErrorDetailModel>
                        {
                             new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                        },
                    };
                }
            }

        }

        public async Task<BaseApiResponseModel> UpdateRentBook(int RentBookId, RentBookModel model)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    model.Id = RentBookId;
                    var RentBook = await _context.RentBook.FirstOrDefaultAsync(x => x.id == RentBookId && !x.deleted);
                    if (RentBook == null)
                    {
                        return new ApiResponseModel<RentBookModel>
                        {
                            Status = false,
                            Message = "RentBook not found",
                            Data = model,
                            HttpStatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, RentBook);
                    await _context.SaveChangesAsync();


                    var oldRentBookDetails = await _context.RentBookDetail.Where(x => x.rent_book_id == RentBook.id).ToListAsync();

                    _context.RentBookDetail.RemoveRange(oldRentBookDetails);
                    await _context.SaveChangesAsync();

                    var newRentBookDetails = _mapper.Map<List<RentBookDetail>>(model.rent_book);
                    foreach (var detail in newRentBookDetails)
                    {
                        detail.rent_book_id = RentBook.id;
                    }

                    await _context.RentBookDetail.AddRangeAsync(newRentBookDetails);
                    await _context.SaveChangesAsync();

                    model = _mapper.Map<RentBookModel>(RentBook);
                    model.rent_book = _mapper.Map<List<RentBookDetailModel>>(newRentBookDetails);

                    await tran.CommitAsync();
                    return new ApiResponseModel<RentBookModel>
                    {
                        Status = true,
                        Data = model,
                        Message = "RentBook updated successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    return new ApiErrorResponseModel<ErrorDetailModel>
                    {
                        Status = false,
                        Message = ex.Message,
                        HttpStatusCode = HttpStatusCode.InternalServerError,
                        Errors = new List<ErrorDetailModel>
                    {
                         new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                    },
                    };
                }

            }
        }

        public async Task<BaseApiResponseModel> ReturnRentBook(int rent_book_id, int[] book_id)
        {

            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!book_id.Any())
                    {
                        return new ApiResponseModel<int[]>()
                        {
                            HttpStatusCode = HttpStatusCode.BadRequest,
                            Status = false,
                            Message = "Retrun book List is required !",
                            Data = book_id
                        };
                    }
                    var rentBookDetail = await _context.RentBookDetail.Where(x => x.rent_book_id == rent_book_id).ToListAsync();

                    if (rentBookDetail.Any())
                    {
                        var check = book_id.Except(rentBookDetail.Select(x => x.book_id).ToArray()).ToArray();
                        if (check.Any())
                        {
                            return new ApiResponseModel<int[]>()
                            {
                                HttpStatusCode = HttpStatusCode.BadRequest,
                                Status = false,
                                Message = "Rent Book Not Found with specific Id!",
                                Data = check
                            };
                        }
                        else
                        {
                            foreach (var item in book_id)
                            {
                                var data = await _context.RentBookDetail
                                    .Where(x => x.book_id == item && x.rent_book_id == rent_book_id)
                                    .ToListAsync();

                                if (data.Any())
                                {
                                    foreach (var rentBook in data)
                                    {
                                        rentBook.return_status = true;
                                    }
                                }
                            }

                            await _context.SaveChangesAsync();

                            await tran.CommitAsync();
                            return new ApiResponseModel<int[]>
                            {
                                Status = true,
                                Data = book_id,
                                Message = "Book Return Status updated successfully",
                                HttpStatusCode = HttpStatusCode.OK
                            };
                        }
                    }


                    else
                    {
                        return new ApiResponseModel<int[]>()
                        {
                            HttpStatusCode = HttpStatusCode.BadRequest,
                            Data = book_id,
                            Status = false,
                            Message = "Rent Book Not Found With Id : " + rent_book_id
                        };
                    }
                }
                catch (Exception ex)
                {


                    await tran.RollbackAsync();
                    return new ApiErrorResponseModel<ErrorDetailModel>
                    {
                        Status = false,
                        Message = ex.Message,
                        HttpStatusCode = HttpStatusCode.InternalServerError,
                        Errors = new List<ErrorDetailModel>
                        {
                             new ErrorDetailModel { Message = ex.InnerException.Message, StackTrace = ex.StackTrace }
                        },
                    };
                }
            }
        }
    }
}
