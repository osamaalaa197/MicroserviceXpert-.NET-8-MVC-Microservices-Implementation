using MicroServiceApplication.Service.CartApi.Dto;

namespace MicroServiceApplication.Service.CartApi.Service.IService
{
	public interface ICouponService
	{
		Task<CouponDto> GetCouponByCouponCode(string code);

	}
}
