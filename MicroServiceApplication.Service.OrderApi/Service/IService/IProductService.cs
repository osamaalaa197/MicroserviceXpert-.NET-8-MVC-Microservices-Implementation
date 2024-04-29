
namespace MicroServiceApplication.Service.OrderApi.Service.IService
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProduct();

	}
}
