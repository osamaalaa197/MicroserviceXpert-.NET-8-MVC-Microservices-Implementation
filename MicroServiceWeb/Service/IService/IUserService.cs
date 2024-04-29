using MicroServiceApplication.CouponAPI.Dto;

namespace MicroServiceWeb
{
	public interface IUserService
	{
		Task<ResponseDto> Register(RegisterDto registerDto);
		Task<ResponseDto> LogIn(LogInDto logInDto);
		Task<ResponseDto> AssignRole(RegisterDto registerDto);


    }
}
