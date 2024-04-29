using MicroServiceApplication.CouponAPI.Dto;

namespace MicroServiceWeb.Service.IService
{
	public interface ICartService
	{
		Task<ResponseDto> GetCartForUser(string UserId);
		Task<ResponseDto> ApplyCoupon(CartDto cartDto);
		Task<ResponseDto> RemoveCoupon(CartDto cartDto);
		Task<ResponseDto> RemoveCatrDetail(int cartDeatail);
		Task<ResponseDto> CartUpsert(CartDto cartDto);
	}
}
