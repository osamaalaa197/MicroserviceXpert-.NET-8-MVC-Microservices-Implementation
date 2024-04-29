namespace MicroServiceApplication.Service.CouponAPI.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }  
        public DateTime CreationDate { get; set; }= DateTime.Now;
        public DateTime? LastUpdatedDate { get; set;}
    }
}
