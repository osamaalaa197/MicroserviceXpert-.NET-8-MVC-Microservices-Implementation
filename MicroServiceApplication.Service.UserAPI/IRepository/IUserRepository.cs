using MicroServiceApplication.MicroServiceApplication.Service.UserAPI;
using MicroServiceApplication.Service.UserAPI.Dto;

namespace MicroServiceApplication.Service.UserAPI.IRepository
{
	public interface IUserRepository
	{
		Task<ResponseDto> Register(RegisterDto registerDto);
		Task<ResponseDto> LogIn(LogInDto logInDto);
		Task<bool> AssignRole(string Email, string RoleName);

	}
}
