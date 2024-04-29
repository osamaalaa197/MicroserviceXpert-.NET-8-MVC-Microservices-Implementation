using MicroServiceApplication.Service.OrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceApplication.Service.OrderApi.Data
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
