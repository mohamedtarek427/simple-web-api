using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ✅ Corrected: DbSet<Product> instead of object
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Category>().ToTable("categories2");
            modelBuilder.Entity<Product>()
               .HasOne(p => p.Category)
               .WithMany()  // If each category can have multiple products
               .HasForeignKey(p => p.categoryId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
