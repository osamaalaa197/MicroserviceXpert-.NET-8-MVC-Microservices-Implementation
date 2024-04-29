using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Const;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service.IService;

namespace MicroServiceWeb.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService=baseService;
        }
        public Task<ResponseDto> CreateOrder(CartDto cartDto)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url=SD.OrderApi + "/api/Order/CreatOrder"
            });
        }

        public Task<ResponseDto> CreateSession(StripeRequestDto stripeRequestDto )
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderApi + "/api/Order/CreateSession"
            });
        }

        public Task<ResponseDto> GetOrderById(int Id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApi + "/api/Order/GetOrderById/"+Id
            });
        }

        public Task<ResponseDto> GetOrderForUser(string userId)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApi + "/api/Order/GetOrderForUser/" +userId
            });
        }

        public Task<ResponseDto> GetOrders()
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApi + "/api/Order/GetOrders"
            });
        }

        public Task<ResponseDto> UpdateOrderStatus(int orderHeaderId, string newStatus)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = newStatus,
                Url = SD.OrderApi + "/api/Order/UpdateOrderStatus/"+orderHeaderId
            });
        }

        public Task<ResponseDto> ValidateSession(int orderHeaderId)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrderApi + "/api/Order/ValidateSession"
            });
        }
    }
}
