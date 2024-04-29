using MicroServiceApplication.Service.CartApi.Dto;

namespace MicroServiceApplication.Service.CartApi.Service.IService
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProduct();

	}
}
