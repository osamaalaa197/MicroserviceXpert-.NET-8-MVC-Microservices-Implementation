using MicroServiceApplication.CouponAPI.Dto;

namespace MicroServiceWeb.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProduct();
        Task<ResponseDto> GetProducById(int id);
        Task<ResponseDto> DeleteProduct(int id);
        Task<ResponseDto> UpdateProduct(ProductDto productDto);
        Task<ResponseDto> AddProduct(ProductDto productDto);
    }
}
