using MicroServiceApplication;
using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Const;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service.IService;

namespace MicroServiceWeb.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService) 
        { 
            _baseService=baseService;
        }

        public Task<ResponseDto> CreateCoupon(CouponDto couponDto)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponApiBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> DeleteCoupon(int Id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponApiBase + "/api/Coupon/Id/" + Id
            });
        }

        public async Task<ResponseDto> GetAllCoupon()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> GetCouponByCouponCode(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/Coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto> GetCouponById(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/Coupon/Id?id=" + couponId
            });
        }

        public async Task<ResponseDto> UpdateCoupon(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.CouponApiBase + "/api/Coupon",
                Data= couponDto 
            });
        }
    }
}
