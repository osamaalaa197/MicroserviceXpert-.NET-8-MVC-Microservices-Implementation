using MicroServiceApplication.Service.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MicroServiceApplication.Service.CouponAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "OO100",
                DiscountAmount = 10,
                MinAmount = 30
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "FF100",
                DiscountAmount = 20,
                MinAmount = 50
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 3,
                CouponCode = "AA105",
                DiscountAmount = 25,
                MinAmount = 80
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 4,
                CouponCode = "BB105",
                DiscountAmount = 30,
                MinAmount = 100
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
