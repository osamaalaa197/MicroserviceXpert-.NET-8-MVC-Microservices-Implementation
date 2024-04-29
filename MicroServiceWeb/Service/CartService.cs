using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Const;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service.IService;

namespace MicroServiceWeb.Service
{
	public class CartService : ICartService
	{
		private readonly IBaseService _baseService;

		public CartService(IBaseService baseService)
        {
            _baseService=baseService;
        }
        public Task<ResponseDto> ApplyCoupon(CartDto cartDto)
		{
			return _baseService.SendAsync(new RequestDto
			{
				Url = SD.CartApi + "/api/CartShopping/ApplyCoupon",
				ApiType = SD.ApiType.POST,
				Data = cartDto
			});
		}

		public Task<ResponseDto> CartUpsert(CartDto cartDto)
		{
			return _baseService.SendAsync(new RequestDto
			{
				Url = SD.CartApi + "/api/CartShopping/CartUpsert",
				ApiType = SD.ApiType.POST,
				Data = cartDto
			});
		}

		public Task<ResponseDto> GetCartForUser(string UserId)
		{
			return _baseService.SendAsync(new RequestDto
			{
				Url=SD.CartApi+ "/api/CartShopping/GetCartForUser/"+ UserId,
				ApiType=SD.ApiType.GET
			});
		}

		public Task<ResponseDto> RemoveCatrDetail(int cartDeatail)
		{
			return _baseService.SendAsync(new RequestDto
			{
				Url = SD.CartApi + "/api/CartShopping/RemoveCartDetails",
				ApiType = SD.ApiType.POST,
				Data=cartDeatail
			});
		}

		public Task<ResponseDto> RemoveCoupon(CartDto cartDto)
		{
			return _baseService.SendAsync(new RequestDto
			{
				Url = SD.CartApi + "/api/CartShopping/RemoveCoupon",
				ApiType = SD.ApiType.POST,
				Data = cartDto
			});
		}
	}
}
