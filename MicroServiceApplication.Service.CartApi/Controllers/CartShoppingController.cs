using MicroServiceApplication.Service.CartApi.Dto;
using MicroServiceApplication.Service.CartApi.Models;
using MicroServiceApplication.Service.CartApi.Repository;
using MicroServiceApplication.Service.CartApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.CartApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartShoppingController : ControllerBase
	{
		private readonly ICartRepository _cartRepository;
		public CartShoppingController(ICartRepository cartRepository)
        {
			_cartRepository= cartRepository;

		}
        [HttpGet("GetCartForUser/{userId}")]
        public async Task<ResponseDto> GetCartForUser(string userId)
		{
			var res= await _cartRepository.GetCartForUser(userId);
			return res;
		}
        [HttpDelete("DeleteCartForUser/{userId}")]
        public ResponseDto DeleteCartForUser(string userId)
        {
            var res = _cartRepository.DeleteCartForUser(userId);
            return res;
        }
        [HttpPost]
		[Route("ApplyCoupon")]
		public async Task<object> ApplyCoupon(CartDto cartDto)
		{
			 var res = await _cartRepository.ApplyCoupon(cartDto);
			return res;
		}
		[HttpPost]
		[Route("RemoveCoupon")]
		public async Task<object> RemoveCoupon(CartDto cartDto)
		{
			var res = await _cartRepository.RemoveCoupon(cartDto);
			return res;
		}
		[HttpPost]
		[Route("RemoveCartDetails")]
		public ResponseDto RemoveCartDetails([FromBody] int cartDetailId)
		{
			var res = _cartRepository.RemoveCartDetails(cartDetailId);
			return res;
		}
		[HttpPost]
		[Route("CartUpsert")]
		public ResponseDto CartUpsert(CartDto cartDto)
		{
			var res = _cartRepository.CartUpsert(cartDto);
			return res;
		}
	}
}
