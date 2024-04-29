using AutoMapper;
using MicroServiceApplication.Service.ProductAPI.Dto;
using MicroServiceApplication.Service.ProductAPI.Models;

namespace MicroServiceApplication.Service.ProductAPI.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

        }
    }
}
