using MicroServiceApplication.Service.CartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceApplication.Service.CartApi.Data
{
	public class CartDbContext:DbContext
	{
		public DbSet<CartHeader> CartHeaders { get; set; }
		public DbSet<CartDetails> CartDetails { get; set; }
		public CartDbContext(DbContextOptions<CartDbContext> options):base(options) { }
	}
}
