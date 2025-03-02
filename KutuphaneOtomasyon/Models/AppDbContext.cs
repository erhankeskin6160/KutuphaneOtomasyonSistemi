using KutuphaneOtomasyon.Models.Models;
 
using Microsoft.EntityFrameworkCore;

namespace KutuphaneOtomasyon.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
       
     

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<BookLoan> BookLoans { get; set; }

        public DbSet<Category> Categories { get; set; }

     

        public DbSet<User> Users { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Notifications> Notifications { get; set; }
    }
}
