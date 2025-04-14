using KutuphaneOtomasyon.Models;

using Microsoft.EntityFrameworkCore;

namespace KutuphaneOtomasyon.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Books",b=>b.HasCheckConstraint("QuantityCheck", "Quantity>=0"));//Bu kodu veritabanında adet sayısı -1 düşmesin diye yapıldı
        }


        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<BookLoan> BookLoans { get; set; }

        public DbSet<Category> Categories { get; set; }



        public DbSet<User> Users { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Notifications> Notifications { get; set; }

        public DbSet<Message> Messages { get; set; }    

        public DbSet<CartItem> CartItems { get; set; }
    }
}
