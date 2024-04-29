using MicroServiceApplication.Service.OrderApi.Const;
using MicroServiceApplication.Service.OrderApi.Dto;
using MicroServiceApplication.Service.OrderApi.IRepository;
using MicroServiceApplication.Service.OrderApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository=orderRepository;
        }
        [HttpPost("CreatOrder")]
        [Authorize]
        public ResponseDto CreatOrder([FromBody] CartDto cart)
        {
            var res=_orderRepository.AddOrder(cart);
            return res;
        }

        [HttpPost("CreateSession")]
        [Authorize]
        public ResponseDto CreateSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            var res = _orderRepository.CreateStripeSession(stripeRequestDto);
            return res;
        }

        [HttpPost("ValidateSession")]
        [Authorize]
        public  async Task<ResponseDto> ValidateSession([FromBody] int orderHeaderId)
        {
            var res =_orderRepository.ValidateSession(orderHeaderId);
            return res;
        }

        [HttpGet("GetOrders")]
        [Authorize(Roles =SD.AdminRole)]
        public ResponseDto GetOrder()
        {
            var res= _orderRepository.GetAllOrder();
            return res;
        }
        [HttpGet("GetOrderById/{OrderId}")]
        [Authorize]
        public ResponseDto GetOrderById(int OrderId)
        {
            var res = _orderRepository.GetOrderById(OrderId);
            return res;
        }

        [HttpGet("GetOrderForUser/{userId}")]
        [Authorize]
        public ResponseDto GetOrderForUser(string userId)
        {
            var res = _orderRepository.GetOrderForUser(userId);
            return res;
        }
        [HttpPost("UpdateOrderStatus/{orderHeaderId}")]
        [Authorize]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto UpdateOrderStatus(int orderHeaderId , [FromBody] string newStatus)
        {
            var res = _orderRepository.UpdateOrderStatus(orderHeaderId,newStatus);
            return res;
        }

    }
}
