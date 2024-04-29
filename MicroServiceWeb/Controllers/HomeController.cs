using MicroServiceWeb.Models;
using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace MicroServiceWeb.Controllers
{
    public class HomeController : Controller
    {
		private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService,ICartService cartService)
		{
			_productService = productService;
            _cartService= cartService;

        }

		public async Task<IActionResult> Index()
        {
			var responseDto = await _productService.GetAllProduct();
			List<ProductDto> data = new();
			if (responseDto != null && responseDto.IsSuccess)
			{
				data = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
				return View(data);
			}
			else
			{
				TempData["error"] = responseDto?.Message;

			}
			return View(data);
		}
		[Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
			var responseDto = await _productService.GetProducById(productId);
			ProductDto data = new();
			if (responseDto != null && responseDto.IsSuccess)
			{
				data = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
				return View(data);
			}
			else
			{
				TempData["error"] = responseDto?.Message;

			}
			return View(data);
		}
		[HttpPost]
		[Authorize]
		[ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
			var cartDto = new CartDto()
			{
				CartHeaderDto = new CartHeaderDto()
				{
					UserId = User.Claims.Where(e => e.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value
				}
			};
			var cartDetail = new CartDetailsDto()
			{
				Count=productDto.Count,
				ProductId=productDto.Id
			};
			var cartDetailDto = new List<CartDetailsDto>();
			cartDetailDto.Add(cartDetail);
			cartDto.CartDetailsDto = cartDetailDto;
			var response=await _cartService.CartUpsert(cartDto);
            if (response!=null && response.IsSuccess)
            {
                TempData["success"] = "Item Added in Shopping Cart Successfully";

                return RedirectToAction(nameof(Index));
			}
			else
			{
                TempData["error"] = response?.Message;

            }

			return View(productDto);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}