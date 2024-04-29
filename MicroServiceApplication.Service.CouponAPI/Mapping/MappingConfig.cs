using AutoMapper;
using MicroServiceApplication.Service.CouponAPI.Dto;
using MicroServiceApplication.Service.CouponAPI.Models;

namespace MicroServiceApplication.Service.CouponAPI.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<CouponDto,Coupon>();
            CreateMap<Coupon, CouponDto>();
        }
    }
}
