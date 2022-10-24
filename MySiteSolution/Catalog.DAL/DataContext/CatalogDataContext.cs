using Microsoft.EntityFrameworkCore;
using Catalog.DAL.Entities;

namespace Catalog.DAL.DataContext
{
    public class CatalogDataContext : DbContext
    {
        public CatalogDataContext(DbContextOptions<CatalogDataContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>();
            builder.Entity<Product>().HasOne(p => p.Category).WithMany(p=>p.Products);
        }
    }
}
