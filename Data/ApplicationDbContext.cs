using LMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        { }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookCategory> BookCategory { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<IssueBook> IssueBook { get; set; }
        public DbSet<Gender> Gender { get; set; }

    }
}
