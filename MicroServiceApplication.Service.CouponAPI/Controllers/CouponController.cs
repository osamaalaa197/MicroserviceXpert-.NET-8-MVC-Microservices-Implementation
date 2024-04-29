using MicroServiceApplication.Service.CouponAPI.Const;
using MicroServiceApplication.Service.CouponAPI.Dto;
using MicroServiceApplication.Service.CouponAPI.IRepository;
using MicroServiceApplication.Service.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository) 
        {
            _couponRepository=couponRepository;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ResponseDto GetAllCoupon() 
        {
           var res= _couponRepository.GetCoupon();
            return res;
        }

        [HttpGet]
        [Route("Id")]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto GetCouponById(int id)
        {
            var res = _couponRepository.GetCouponById(id);
            return res;
        }
        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto AddCoupon(CouponDto coupon) 
        {
            var res= _couponRepository.AddCoupon(coupon);
            return res;
        }
        [HttpGet]
        [Route("GetByCode/{couponCode}")]
        public ResponseDto GetCouponByCouponCode(string couponCode) 
        {
            var res=_couponRepository.GetCouponByCouponCode(couponCode);
            return res;
        }
        [HttpPut]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto UpdateCoupon([FromBody]CouponDto coupon) 
        {
            var res= _couponRepository.UpdateCoupon( coupon);
            return res;
        }
        [HttpDelete]
        [Route("Id/{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto Delete(int id)
        {
            var res = _couponRepository.DeleteCoupon(id);
            return res;
        }
    }
}
