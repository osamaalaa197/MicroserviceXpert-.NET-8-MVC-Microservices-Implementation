using MicroServiceApplication;
using MicroServiceApplication.CouponAPI.Dto;

namespace MicroServiceWeb.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetAllCoupon();
        Task<ResponseDto> GetCouponById(int couponId);
        Task<ResponseDto> UpdateCoupon(CouponDto couponDto);
        Task<ResponseDto> CreateCoupon(CouponDto couponDto);

        Task<ResponseDto> GetCouponByCouponCode(string couponCode);
        Task<ResponseDto> DeleteCoupon(int id);


    }
}
