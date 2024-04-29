using MicroServiceWeb.Const;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace MicroServiceWeb.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService,IOrderService orderService)
        {
			_cartService = cartService;
            _orderService=orderService;

        }
		[Authorize]
        public async Task <IActionResult> CartIndex()
		{
			string userId = User.Claims.Where(e => e.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
			var res = await _cartService.GetCartForUser(userId);
			if (res != null && res.IsSuccess) 
			{ 
				var cartDto=JsonConvert.DeserializeObject<CartDto>(Convert.ToString(res.Result.ToString()));
				return View(cartDto);

			}
			return View(new CartDto());
		}
        public async Task<IActionResult> RemoveItemFromCart(int cartDetailsId)
        {
            var res = await _cartService.RemoveCatrDetail(cartDetailsId);
            if (res != null && res.IsSuccess)
            {

                TempData["success"] = "Item deleted Succssfully";
                return RedirectToAction(nameof(CartIndex));

			}
			else
			{
                TempData["error"] = res?.Message;

            }
            return View();
        }
		[Authorize]
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var res = await _cartService.ApplyCoupon(cartDto);
            if (res != null && res.IsSuccess)
            {
                TempData["success"] = "Cart Updated Succssfully";
                return RedirectToAction(nameof(CartIndex));

            }
            else
            {
                TempData["error"] = res?.Message;
                return RedirectToAction(nameof(CartIndex));

            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var res = await _cartService.RemoveCoupon(cartDto);
            if (res != null && res.IsSuccess)
            {
                TempData["success"] = "Cart Updated Succssfully";
                return RedirectToAction(nameof(CartIndex));

            }
            else
            {
                TempData["error"] = res?.Message;

            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            string userId = User.Claims.Where(e => e.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
            var res = await _cartService.GetCartForUser(userId);
            if (res != null && res.IsSuccess)
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(res.Result.ToString()));
                return View(cartDto);

            }
            return View(new CartDto());
        }
        [Authorize]
        [ActionName("CheckOut")]
        [HttpPost]
        public async Task<IActionResult> CheckOut(CartDto cart)
        {
            string userId = User.Claims.Where(e => e.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
            var res = await _cartService.GetCartForUser(userId);
            var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(res.Result.ToString()));
            cartDto.CartHeaderDto.Name = cart.CartHeaderDto.Name;
            cartDto.CartHeaderDto.Phone = cart.CartHeaderDto.Phone;
            cartDto.CartHeaderDto.Email = cart.CartHeaderDto.Email;
            var response=await _orderService.CreateOrder(cartDto);
            var orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            if (response.Result !=null && response.IsSuccess)
            {
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                var stripeRequestDto = new StripeRequestDto()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CanceledUrl = domain + "cart/CheckOut",
                    OrderHeaderDto = orderHeaderDto
                };
                var responseDto = await _orderService.CreateSession(stripeRequestDto);
                var striperesponse = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(responseDto.Result));
                Response.Headers.Add("Location", striperesponse.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            return View(cart);
        }
        public async Task<IActionResult> Confirmation(int orderId)
        {
            //redirect to some error page based on status
            var res=await _orderService.ValidateSession(orderId);
            var orderaheder=JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(res.Result));
            if (orderaheder.Status==SD.Status_Approved)
            {
                return View(orderId);
            }
            return View(nameof(CheckOut));

        }

    }
}
