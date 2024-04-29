using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Models;

namespace MicroServiceWeb.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto>CreateOrder(CartDto cartDto);
        Task<ResponseDto> CreateSession(StripeRequestDto stripeRequestDto);
        Task<ResponseDto> ValidateSession(int orderHeaderId);
        Task<ResponseDto> UpdateOrderStatus(int orderHeaderId, string newStatus);
        Task<ResponseDto> GetOrders();
        Task<ResponseDto> GetOrderById(int Id);
        Task<ResponseDto> GetOrderForUser(string userId);

    }
}
