using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class TransactionRepository : ITransaction
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<TransactionRepository> _logger;
        private string _userId;
        public TransactionRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, ILogger<TransactionRepository> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger = logger;
        }
        public async Task<List<TransactionGETModel>> GetAllTransaction()
        {
            return await _context.Transaction.Where(x => x.Deleted == false).Select(x => new TransactionGETModel
            {
                Id = x.Id,
                Remarks = x.Remarks,
                IssuedDate = x.CreatedDate.Date,
                BookTransactionList = _context.BookTransaction.Where(z => z.TransactionId == x.Id).Select(z => new BookTransactionGETModel()
                {
                    Id = z.Id,
                    TransactionId = z.TransactionId,
                    BookId = z.BookId,
                    BookName = z.Book.BookName,
                    StudentId = z.StudentId,
                    StudentName = z.Student.FirstName + " " + z.Student.LastName,
                    Status = z.Status,
                    StatusName = z.Status.ToString(),

                }).ToList(),
                CreatedBy = _context.Users.Where(z => z.Id == x.CreatedBy).Select(z => (z.FirstName + " " + z.LastName)).FirstOrDefault() ?? "Admin"


            }).ToListAsync();
        }

        public async Task<TransactionGETModel> GetTransactionById(int id)
        {
            return await _context.Transaction.Where(x => x.Id == id && x.Deleted == false).Select(x => new TransactionGETModel()
            {
                Id = x.Id,
                Remarks = x.Remarks,
                IssuedDate = x.CreatedDate.Date,
                BookTransactionList = _context.BookTransaction.Where(z => z.TransactionId == x.Id).Select(z => new BookTransactionGETModel()
                {
                    Id = z.Id,
                    TransactionId = z.TransactionId,
                    BookId = z.BookId,
                    BookName = z.Book.BookName,
                    StudentId = z.StudentId,
                    StudentName = z.Student.FirstName + " " + z.Student.LastName,
                    Status = z.Status,
                    StatusName = z.Status.ToString(),

                }).ToList(),
                CreatedBy = _context.Users.Where(z => z.Id == x.CreatedBy).Select(z => (z.FirstName + " " + z.LastName)).FirstOrDefault() ?? "Admin"


            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateTransaction(TransactionModel model)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model.Id > 0)
                    {
                        var Transaction = await _context.Transaction.Where(x => x.Id == model.Id && x.Deleted == false).FirstOrDefaultAsync();
                        if (Transaction != null)
                        {
                            Transaction.Remarks = model.Remarks;
                            Transaction.UpdatedBy = _userId;
                            Transaction.UpdatedDate = DateTime.UtcNow;
                            _context.Entry(Transaction).State = EntityState.Modified;
                            if (model.BookTransactionList.Count > 0)
                            {
                                foreach (var item in model.BookTransactionList)
                                {
                                    var bookTransaction = await _context.BookTransaction.Where(x => x.TransactionId == Transaction.Id && x.Id == item.Id).FirstOrDefaultAsync();
                                    if (bookTransaction != null)
                                    {
                                        bookTransaction.ReturnDate = item.ReturnDate;
                                        bookTransaction.BookId = item.BookId;
                                        bookTransaction.Status = item.Status;
                                        bookTransaction.StudentId = item.StudentId;
                                        _context.Entry(bookTransaction).State = EntityState.Modified;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Transaction Transaction = new Transaction()
                        {
                            Remarks = model.Remarks,
                            CreatedBy = _userId,
                            CreatedDate = DateTime.UtcNow,
                            Deleted = false
                        };
                        await _context.Transaction.AddAsync(Transaction);
                        await _context.SaveChangesAsync();
                        if (model.BookTransactionList.Count > 0)
                        {
                            foreach (var item in model.BookTransactionList)
                            {
                                BookTransaction bookTransaction = new BookTransaction()
                                {
                                    TransactionId = Transaction.Id,
                                    BookId = item.BookId,
                                    Status = TransactionStatus.Issued,
                                    StudentId = item.StudentId,
                                    ReturnDate = item.ReturnDate,
                                };
                                await _context.BookTransaction.AddAsync(bookTransaction);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error Occured, DateTime:{DateTime.UtcNow}, ErrorDescription:{ex}");
                    await dbTransaction.RollbackAsync();
                    return false;
                }

            }
        }
        public async Task<bool> DeleteTransaction(int id)
        {
            try
            {
                var Transaction = await _context.Transaction.FindAsync(id);
                if (Transaction != null)
                {
                    Transaction.Deleted = true;
                    Transaction.DeletedDate = DateTime.UtcNow;
                    Transaction.DeletedBy = _userId;
                    _context.Entry(Transaction).State = EntityState.Modified;
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
