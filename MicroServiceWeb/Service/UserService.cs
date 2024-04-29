using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Const;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service.IService;
using static System.Net.WebRequestMethods;

namespace MicroServiceWeb.Service
{
	public class UserService: IUserService
	{
		private readonly IBaseService _baseService;

		public UserService(IBaseService baseService) 
		{
			_baseService=baseService;
		}

		public Task<ResponseDto> LogIn(LogInDto logInDto)
		{
			return _baseService.SendAsync(new RequestDto
			{
				ApiType = Const.SD.ApiType.POST,
				Data = logInDto,
				Url = SD.UserApiBase + "/api/User/LogIn"
			});
		}

		public Task<ResponseDto> Register(RegisterDto registerDto)
		{
			return _baseService.SendAsync(new RequestDto
			{
				ApiType = Const.SD.ApiType.POST,
				Data = registerDto,
				Url = SD.UserApiBase + "/api/User/Register"
			});
		}

        public Task<ResponseDto> AssignRole(RegisterDto registerDto)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = Const.SD.ApiType.POST,
                Data = registerDto,
                Url = SD.UserApiBase + "/api/User/AssignRole"
            });
        }
    }
}
