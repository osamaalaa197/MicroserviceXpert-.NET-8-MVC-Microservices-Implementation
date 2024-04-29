using MicroServiceApplication.MicroServiceApplication.Service.UserAPI;
using MicroServiceApplication.Service.UserAPI.Data;
using MicroServiceApplication.Service.UserAPI.Dto;
using MicroServiceApplication.Service.UserAPI.IRepository;
using MicroServiceApplication.Service.UserAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroServiceApplication.Service.UserAPI.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly UserContext _userContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _jWTTokenGenerator;

		public UserRepository(UserContext userContext,UserManager<ApplicationUser> userManager,IConfiguration configuration,RoleManager<IdentityRole> roleManager,IJWTTokenGenerator jWTTokenGenerator) 
		{
			_userContext=userContext;
			_userManager=userManager;
			_configuration = configuration;
			_roleManager=roleManager;
			_jWTTokenGenerator = jWTTokenGenerator;
        }

		public async Task<bool> AssignRole(string Email, string RoleName)
		{
			var user=await _userManager.FindByEmailAsync(Email);
            if (user!=null)
            {
				var isExists=await _roleManager.RoleExistsAsync(RoleName);
                if (!isExists)
                {
					await _roleManager.CreateAsync(new IdentityRole(RoleName));
                }
				await _userManager.AddToRoleAsync(user, RoleName);
				return true;
            }
			return false;
        }

		public async Task<ResponseDto> LogIn(LogInDto logInDto)
		{
			var response = new ResponseDto();
			var User = await _userManager.FindByEmailAsync(logInDto.Email);
			if(User == null) 
			{
				response.IsSuccess = false;
				response.Message = "Email not Valid ";
				return response;
			}
			var isexist= await _userManager.CheckPasswordAsync(User, logInDto.Password);
			if (!isexist)
			{
				response.IsSuccess = false;
				response.Message = "Wrong Password ";
				return response;
			}
			var userRole = await _userManager.GetRolesAsync(User);
            var token = _jWTTokenGenerator.GenerateToken(User,userRole);
			response.Result = new LogInResponseDto
			{
				Token = token,
				UserId = User.Id
			};
			return response;
		}

		public async Task<ResponseDto> Register(RegisterDto registerDto)
		{
			var response=new ResponseDto();
			var appUser = new ApplicationUser();
			appUser.UserName = registerDto.Name;
			appUser.Email= registerDto.Email;
			appUser.PhoneNumber= registerDto.PhoneNumber;
			var result= await _userManager.CreateAsync(appUser,registerDto.Password);
			if(!result.Succeeded) 
			{
				response.IsSuccess = false;
				response.Message=result.Errors.FirstOrDefault().Description;
				return response;
			}
			response.IsSuccess = true;
			response.Message = "Account Register Successfully";
            return response;
		}
	}
}
