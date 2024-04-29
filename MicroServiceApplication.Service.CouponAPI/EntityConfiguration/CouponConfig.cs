using MicroServiceApplication.Service.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroServiceApplication.Service.CouponAPI.EntityConfiguration
{
    public class CouponConfig:IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.Property(l => l.CouponCode).HasMaxLength(150);
            builder.Property(l=>l.CouponId).IsRequired();
            builder.Property(l => l.CouponCode).IsRequired();
            builder.Property(l => l.DiscountAmount).IsRequired();
            builder.Property(l => l.MinAmount).IsRequired();

        }
    }
}
