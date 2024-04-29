using MicroServiceApplication.Service.CouponAPI.Dto;
using MicroServiceApplication.Service.CouponAPI.Models;

namespace MicroServiceApplication.Service.CouponAPI.IRepository
{
    public interface ICouponRepository
    {
        ResponseDto GetCoupon();
        ResponseDto AddCoupon (CouponDto coupon);
        ResponseDto UpdateCoupon (CouponDto coupon);
        ResponseDto GetCouponById (int Id);
        ResponseDto GetCouponByCouponCode(string couponCode);
        ResponseDto DeleteCoupon(int Id);


    }
}
