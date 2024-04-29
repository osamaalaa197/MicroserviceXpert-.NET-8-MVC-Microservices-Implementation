using AutoMapper;
using MicroServiceApplication.Service.OrderApi.Dto;
using MicroServiceApplication.Service.OrderApi.Models;

namespace MicroServiceApplication.Service.OrderApi.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<OrderHeader,OrderHeaderDto>().ForMember(dest=>dest.OrderHeaderId,u=>u.MapFrom(src=>src.Id)).ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ForMember(dest => dest.OrderDetailId, u => u.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<OrderHeaderDto, CartHeaderDto>()
            .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

            CreateMap<CartDetailsDto, OrderDetailsDto>()
            .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.ProductDto.Name))
            .ForMember(dest => dest.Price, u => u.MapFrom(src => src.ProductDto.Price));

            CreateMap<OrderDetailsDto,CartDetailsDto>().ReverseMap();
        }
    }
}
