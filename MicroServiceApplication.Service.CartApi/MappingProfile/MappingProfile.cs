using AutoMapper;
using MicroServiceApplication.Service.CartApi.Dto;
using MicroServiceApplication.Service.CartApi.Models;

namespace MicroServiceApplication.Service.CartApi.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();

        }
    }
}
