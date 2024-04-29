using MicroServiceApplication.Service.CartApi.Dto;
using MicroServiceApplication.Service.CartApi.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;

namespace MicroServiceApplication.Service.CartApi.Service
{
	public class CouponService : ICouponService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CouponService(IHttpClientFactory httpClientFactory) 
		{
			_httpClientFactory=httpClientFactory;
		}
		public async Task<CouponDto> GetCouponByCouponCode(string code)
		{
			var clint = _httpClientFactory.CreateClient("Coupon");
			var response = await clint.GetAsync($"api/Coupon/GetByCode/{code}");
			var apiContent= await response.Content.ReadAsStringAsync();	
			var data=JsonConvert.DeserializeObject<ResponseDto>(apiContent);
			if (data !=null && data.IsSuccess==true) 
			{
				var couponData = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(data.Result));
				return couponData;
			}
			return new CouponDto();
		}
	}
}
