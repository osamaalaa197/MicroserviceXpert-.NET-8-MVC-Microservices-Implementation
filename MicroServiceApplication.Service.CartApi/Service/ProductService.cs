using MicroServiceApplication.Service.CartApi.Dto;
using MicroServiceApplication.Service.CartApi.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace MicroServiceApplication.Service.CartApi.Service
{
	public class ProductService : IProductService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProductService(IHttpClientFactory httpClientFactory) 
		{
			_httpClientFactory=httpClientFactory;
		}
		public async Task<List<ProductDto>> GetAllProduct()
		{
			var clinet = _httpClientFactory.CreateClient("Product");
			var response = await clinet.GetAsync($"api/Product");
			var content= await response.Content.ReadAsStringAsync();
			var repo=JsonConvert.DeserializeObject<ResponseDto>(content);
            if (repo.IsSuccess==true)
            {
				var ProductDto = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(repo.Result));
				return ProductDto;
            }
			return new List<ProductDto>();
		}
	}
}
