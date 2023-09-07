using LMS.Data;
using LMS.Interface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository
{
    public class CommonRepository : ICommon
    {
        private readonly ApplicationDbContext _context;

        public CommonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommonModel>> GetGender()
        {
            return await _context.Gender.Select(x => new CommonModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }
    }
}
