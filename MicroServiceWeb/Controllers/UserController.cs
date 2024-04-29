using MicroServiceWeb.Const;
using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace MicroServiceWeb.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;

        public UserController(IUserService userService,ITokenProvider tokenProvider) 
		{
			_userService=userService;
            _tokenProvider=tokenProvider;   

        }
		public async Task<IActionResult> Register()
		{
            var selectList = new List<SelectListItem>()
            {
               new SelectListItem() {Text=SD.Customer,Value=SD.Customer},
                new SelectListItem() {Text=SD.AdminRole,Value=SD.AdminRole},
            };
            ViewBag.RoleList = selectList;
            return View();
		}
		[HttpPost]
        public async Task<IActionResult> SaveAccount(RegisterDto registerDto)
        {
			if(!ModelState.IsValid) 
			{

                ViewBag.RoleList = GetRoleList();
                return View("Register", registerDto);
            }
            var res = await _userService.Register(registerDto);
            if (res.IsSuccess && res != null)
            {
                if (string.IsNullOrEmpty(registerDto.Role))
                {
                    registerDto.Role = SD.Customer;
                }
                var assignRole = await _userService.AssignRole(registerDto);
                if (assignRole != null && assignRole.IsSuccess)
                {

                    TempData["success"] = res.Message;
                    return RedirectToAction("LogIn");
                }
                else
                {
                    TempData["error"] = assignRole.Message;
                    ViewBag.RoleList = GetRoleList();
                    return View("Register", registerDto);
                }
            }
            else
            {
                TempData["error"] = res.Message;
            }
            ViewBag.RoleList = GetRoleList();
            return View("Register", registerDto);

        }
        public async Task<IActionResult> LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveLogIn(LogInDto logInDto)
        {
            if (!ModelState.IsValid)
            {
                return View("LogIn", logInDto);

            }
            var res = await _userService.LogIn(logInDto);
            if(res.Result !=null && res.IsSuccess)
            {
                LogInResponseDto logInResponseDto=JsonConvert.DeserializeObject<LogInResponseDto>(Convert.ToString(res.Result));
                _tokenProvider.SetToken(logInResponseDto.Token);
                await SignInUser(logInResponseDto);
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = res.Message;
            return View("LogIn", logInDto);
        }

        private async Task SignInUser(LogInResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.CleanToken();
            return RedirectToAction("Index", "Home");
        }
        private List<SelectListItem> GetRoleList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Text = SD.AdminRole, Value = SD.AdminRole },
        new SelectListItem { Text = SD.Customer, Value = SD.Customer }
    };
        }
    }
}
