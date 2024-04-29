using MicroServiceApplication.MicroServiceApplication.Service.UserAPI;
using MicroServiceApplication.Service.UserAPI.Dto;
using MicroServiceApplication.Service.UserAPI.IRepository;
using MicroServiceApplication.Service.UserAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.UserAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository) 
		{
			_userRepository=userRepository;
		}
		[HttpPost]
		[Route("Register")]
		public Task<ResponseDto> Register([FromBody] RegisterDto registerDto)
		{
			var res = _userRepository.Register(registerDto);
			return res;
		}

		[HttpPost]
		[Route("LogIn")]
		public Task<ResponseDto> LogIn([FromBody] LogInDto logInDto )
		{
			var res = _userRepository.LogIn(logInDto);
			return res;
		}
		[HttpPost]
		[Route("AssignRole")]
		public async Task<ResponseDto> AssignRole(RegisterDto registerDto)
		{
			var res = await _userRepository.AssignRole(registerDto.Email,registerDto.Role.ToUpper());
			var response=new ResponseDto();
			response.Result = res;
			return response;
		}
	}
}
