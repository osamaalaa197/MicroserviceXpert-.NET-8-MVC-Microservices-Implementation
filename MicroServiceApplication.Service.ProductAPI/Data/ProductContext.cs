using MicroServiceApplication.Service.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceApplication.Service.ProductAPI.Data
{
    public class ProductContext:DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> option):base(option) 
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
