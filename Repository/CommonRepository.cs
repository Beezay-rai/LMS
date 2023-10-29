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
        public async Task<List<CommonModel>> GetRoles()
        {
            return await _context.Roles.Where(x=>x.NormalizedName != "ADMINISTRATOR").Select(x => new CommonModel()
            {
                Name = x.Name,
            }).ToListAsync();
        }
        public async Task<List<GETSignUpModel>> GetAllUser()
        {
            var data= await _context.Users.Select(x => new GETSignUpModel()
            {
                FirstName = x.FirstName,
                LastName=x.LastName,
                Email = x.Email,
                Role = (from r in _context.Roles 
                       join ur in _context.UserRoles on r.Id equals ur.RoleId
                       where ur.UserId == x.Id
                       select r.Name) .FirstOrDefault(),
                Active = x.Active
            }).ToListAsync();
            return data;
        }
    }
}
