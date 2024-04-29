using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Const;
using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace MicroServiceWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService=orderService;
        }
        public IActionResult OrderIndex()
        {
            return View();
        }
        [HttpGet]
        public  IActionResult GetAllOrder(string status) 
         {
            IEnumerable<OrderHeaderDto> list=null;
            if (User.IsInRole(SD.AdminRole))
            {
                var res = _orderService.GetOrders().GetAwaiter().GetResult();
                if (res.Result != null && res.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(res.Result));
                    switch (status)
                    {
                        case SD.Status_Approved:
                            list=list.Where(e=>e.Status == SD.Status_Approved);
                            break;
                        case SD.Status_Completed:
                            list = list.Where(e => e.Status == SD.Status_Completed);
                            break;
                        case SD.Status_ReadyForPickUp:
                            list = list.Where(e => e.Status == SD.Status_ReadyForPickUp);
                            break;
                        case SD.Status_Cancelled:
                            list = list.Where(e => e.Status == SD.Status_Cancelled);
                            break;
                        case SD.Status_Pending:
                            list = list.Where(e => e.Status == SD.Status_Pending);
                            break;
                        default
                            : break;
                    }
                        
                }
            }
            else
            {
                var userId = User.Claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Sub).Value;
                var obj = _orderService.GetOrderForUser(userId).GetAwaiter().GetResult();
                if (obj.Result != null && obj.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(obj.Result));

                }
            }
            
            return Json(new {data=list});
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderDetail(int id) 
        {
            var res=await _orderService.GetOrderById(id);
            if (res.Result!=null&&res.IsSuccess)
            {
                var data=JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(res.Result));
                return View(data);
            }
            return View(null);
        }
        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int id)
        {
            var res = await _orderService.UpdateOrderStatus(id, SD.Status_ReadyForPickUp);
            if (res.Result != null && res.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(GetOrderDetail), new { id = id });
            }
            return View();
        }
        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var res = await _orderService.UpdateOrderStatus(id, SD.Status_Completed);
            if (res.Result != null && res.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(GetOrderDetail), new { id = id });
            }
            return View();
        }
        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var res = await _orderService.UpdateOrderStatus(id, SD.Status_Cancelled);
            if (res.Result != null && res.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(GetOrderDetail), new { id = id });
            }
            return View();
        }
    }
}
