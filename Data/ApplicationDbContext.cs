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
        public DbSet<Student> Student { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Staff> Staff { get; set; }
<<<<<<< HEAD
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<BookTransaction> BookTransaction { get; set; }
        public DbSet<BookCategoryDetail> BookCategoryDetail { get; set; }
        public DbSet<Course> Course { get; set; }
=======
        public DbSet<IssueBook> IssueBook { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        public DbSet<Gender> Gender { get; set; }

    }
}
