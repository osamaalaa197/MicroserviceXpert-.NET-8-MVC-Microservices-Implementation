using MicroServiceApplication.Service.OrderApi.Dto;
using MicroServiceApplication.Service.OrderApi.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;

namespace MicroServiceApplication.Service.OrderApi.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory=httpClientFactory;
        }
        public async Task<ResponseDto> DeleteUserCart(string userId)
        {
            var clinet = _httpClientFactory.CreateClient("Cart");
            var response = await clinet.DeleteAsync($"api/CartShopping/DeleteCartForUser/{userId}");
            var content = await response.Content.ReadAsStringAsync();
            var repo = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (repo.IsSuccess == true)
            {
                var responseDto = JsonConvert.DeserializeObject<ResponseDto>(Convert.ToString(repo.Result));
                return responseDto;
            }
            return new ResponseDto();
        }
    }
}
