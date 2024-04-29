using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Const;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service.IService;

namespace MicroServiceWeb.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService=baseService;
        }
        public Task<ResponseDto> AddProduct(ProductDto productDto)
        {
            return _baseService.SendAsync(new RequestDto
            {
                Data = productDto,
                ApiType = SD.ApiType.POST,
                Url = SD.ProductApiBase + "/api/Product",
                ContentType=SD.ContentType.MultipartFormData,
            });
        }

        public Task<ResponseDto> DeleteProduct(int id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductApiBase + "/api/Product/Delete/"+id
            });
        }

        public Task<ResponseDto> GetAllProduct()
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/Product"
            });
        }

        public Task<ResponseDto> GetProducById(int id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/Product/GetById/" + id
            });
        }

        public Task<ResponseDto> UpdateProduct(ProductDto productDto)
        {
            var request = new RequestDto
            {
                Data = productDto,
                ApiType = SD.ApiType.PUT,
                Url = SD.ProductApiBase + "/api/Product",
            };
            if (productDto.Image!=null)
            {
                request.ContentType = SD.ContentType.MultipartFormData;
                return _baseService.SendAsync(request);
            }
            else
            {
                return _baseService.SendAsync(request);
            }

        }
    }
}
