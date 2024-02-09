using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models
{
    public class ProductsContext:DbContext
    {
        
        public ProductsContext(DbContextOptions<ProductsContext> options): base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 1, ProductName = "iphone 14", Price = 12000, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 2, ProductName = "iphone 15", Price = 14000, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 3, ProductName = "iphone 16", Price = 18000, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product() { ProductId = 4, ProductName = "iphone 17", Price = 20000, IsActive = false });

        }

        public DbSet<Product> Products { get; set; }


    }
}
