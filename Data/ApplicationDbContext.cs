using LMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Book> Book { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<BookCategory> BookCategory { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<BookTransaction> BookTransaction { get; set; }
        public DbSet<BookCategoryDetail> BookCategoryDetail { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<RentBook> RentBook { get; set; }
        public DbSet<RentBookDetail> RentBookDetail { get; set; }

    }
}
