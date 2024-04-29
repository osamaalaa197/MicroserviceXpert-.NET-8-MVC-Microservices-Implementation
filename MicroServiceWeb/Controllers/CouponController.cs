using MicroServiceApplication;
using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MicroServiceWeb.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService) 
        {
            _couponService=couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list =new();
            ResponseDto? response=await _couponService.GetAllCoupon();
            if (response != null && response.IsSuccess)
            {
                list=JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
                return View(list);
            }
            else
            {
                TempData["error"] = response?.Message;

            }
            return View(list);
        }

        public async Task<IActionResult> CreateCoupon() 
        {
            var couponDto=new CouponDto()
            {
                Header= "Create Coupon",
                Action= "SaveCoupon",
                ButtonTitle="Create"

            };
            return View("_FormCoupon",couponDto);
        }
        [HttpPost]
		public async Task<IActionResult> SaveCoupon(CouponDto couponDto)
		{
            if(ModelState.IsValid) 
            {
				var res = await _couponService.CreateCoupon(couponDto);
				if (res != null && res.IsSuccess)
				{
                    TempData["success"] = res.Message;
                    return RedirectToAction("CouponIndex");
                }
                TempData["error"] = res.Message;

            }
            return View("_FormCoupon", couponDto);
        }
        [HttpGet]
		public async Task<IActionResult> DeleteCoupon(int id)
		{
			var responseDto = await _couponService.GetCouponById(id);
			if (responseDto != null && responseDto.IsSuccess)
            {
                var couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
                couponDto.Header = "Delete Coupon";
                couponDto.Action = "RemoveCoupon";
                couponDto.ButtonTitle = "Delete";
                return View("_FormCoupon", couponDto);
            }
			TempData["error"] = responseDto.Message;
			return NotFound();
		}
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _couponService.DeleteCoupon(couponDto.CouponId);
                if (res != null && res.IsSuccess)
                {
                    TempData["success"] = res.Message;
                    return RedirectToAction("CouponIndex");
                }
                TempData["error"] = res.Message;

            }
            return View("_FormCoupon", couponDto);
        }

		[HttpGet]
		public async Task<IActionResult> UpdateCoupon(int id)
		{
			var responseDto = await _couponService.GetCouponById(id);
            if(responseDto != null && responseDto.IsSuccess)
            {
				var couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
				couponDto.Header = "Update Coupon";
				couponDto.Action = "SaveUpdate";
				couponDto.ButtonTitle = "Save";
				return View("_FormCoupon", couponDto);
			}
			TempData["error"] = responseDto.Message;
			return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> SaveUpdate(CouponDto couponDto)
        {
			if (ModelState.IsValid)
			{
				var res = await _couponService.UpdateCoupon(couponDto);
				if (res != null && res.IsSuccess)
				{
					TempData["success"] = res.Message;
					return RedirectToAction("CouponIndex");
				}
                TempData["error"] = res.Message;
            }
            return View("_FormCoupon", couponDto);
		}

	}
}
