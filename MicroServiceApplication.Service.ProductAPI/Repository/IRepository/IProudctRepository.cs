using MicroServiceApplication.Service.ProductAPI.Dto;

namespace MicroServiceApplication.Service.ProductAPI.Repository.IRepository
{
    public interface IProudctRepository
    {
        ResponseDto AddProduct(ProductDto productDto,string baseurl);
        ResponseDto GetProductById(int productId);
        ResponseDto DeleteProduct(int productId);
        ResponseDto UpdateProduct(ProductDto productDto,string baseUrl);
        ResponseDto GetAllProduct();


    }
}
