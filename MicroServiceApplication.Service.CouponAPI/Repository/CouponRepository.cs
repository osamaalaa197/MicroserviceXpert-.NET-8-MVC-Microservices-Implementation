using AutoMapper;
using MicroServiceApplication.Service.CouponAPI.Data;
using MicroServiceApplication.Service.CouponAPI.Dto;
using MicroServiceApplication.Service.CouponAPI.IRepository;
using MicroServiceApplication.Service.CouponAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace MicroServiceApplication.Service.CouponAPI.Repository
{
    public class CouponRepository:ICouponRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(AppDbContext appDbContext,IMapper mapper) 
        {
            _context=appDbContext;
            _mapper=mapper;
        }

        public ResponseDto AddCoupon([FromBody] CouponDto coupon)
        {
            try 
            {
                var newCoupon = new Coupon();
                var res = _mapper.Map(coupon, newCoupon);
                _context.Coupons.Add(res);
                _context.SaveChanges();

                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff =(long)(res.DiscountAmount*100),
                    Name=res.CouponCode,
                    Currency="usd",
                    Id=res.CouponCode
                };
                var service = new  Stripe.CouponService();
                service.Create(options);
                var response = new ResponseDto()
                {
                    Result= res,
                    Message="Coupon add Successfully"
                };
                return response;
            }
            catch (Exception ex) 
            {
                var response=new ResponseDto()
                {
                    Message = ex.Message,
                    IsSuccess=false,
                };
                return response;
            }


        }

        public ResponseDto DeleteCoupon(int Id)
        {
            var response = new ResponseDto();
            try
            {
                var coupon = _context.Coupons.First(e => e.CouponId == Id);
                _context.Coupons.Remove(coupon);
                _context.SaveChanges();
                var service = new Stripe.CouponService();
                service.Delete(coupon.CouponCode);
                response.IsSuccess = true;
                response.Message = "Coupon Deleted Successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto GetCoupon()
        {
            try 
            { 
                var coupons=_context.Coupons.ToList();
                var couponsDto=new List<CouponDto>();
                var response = new ResponseDto()
                {
                    Result = _mapper.Map(coupons, couponsDto),
                };
                return response;
            }
            catch (Exception ex) 
            {
                var response = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
                return response;
            }
        }

        public ResponseDto GetCouponByCouponCode(string couponCode)
        {
            var response = new ResponseDto();
            try
            {
                var res = _context.Coupons.First(e => e.CouponCode.ToLower() == couponCode.ToLower());
                var couponsDto = new CouponDto();
                _mapper.Map(res, couponsDto);
                response.Result = couponsDto;
            }
            catch (Exception ex)
            {
                response.Message= ex.Message;
                response.IsSuccess=false;
            }
            return response;
        }

        public ResponseDto GetCouponById(int Id)
        {
            var response = new ResponseDto();
            try
            {
                var coupon = _context.Coupons.First(e => e.CouponId == Id);
                response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex) 
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto UpdateCoupon(CouponDto couponDto)
        {
            try
            {
                var res=_mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(res);
                _context.SaveChanges();
                var response = new ResponseDto()
                {
                    Result = res,
                    Message = "Coupon Updated Successfully"

                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
                return response;
            }

        }
    }
}
