using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Admin.Repository
{
    public class AuthorRepository : IAuthor
    {
        private readonly ApplicationDbContext _context;
        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorViewModel>> GetAllAuthor()
        {
            return await _context.Author.Select(x => new AuthorViewModel
            {
                Id = x.Id,
                Name = x.Name,
                BirthDate = x.BirthDate
            }).ToListAsync();
        }

        public async Task<AuthorViewModel> GetAuthorById(int id)
        {
            return await _context.Author.Where(x => x.Id == id).Select(x => new AuthorViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                BirthDate = x.BirthDate
            }).FirstOrDefaultAsync() ?? new AuthorViewModel();
        }
        public async Task<bool> CreateAuthor(AuthorViewModel model)
        {
            try
            {
                Author author = new Author()
                {
                    Name = model.Name,
                    BirthDate = model.BirthDate
                };
                await _context.Author.AddAsync(author);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> EditAuthor(AuthorViewModel model)
        {
            try
            {
                var author = await _context.Author.FindAsync(model.Id);
                if (author != null)
                {
                    author.Name = model.Name;
                    author.BirthDate = model.BirthDate;
                    _context.Entry(author).State = EntityState.Modified;
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
        public async Task<bool> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Author.FindAsync(id);
                if (author != null)
                {
                    author.Deleted = true;
                    _context.Entry(author).State = EntityState.Modified;
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
