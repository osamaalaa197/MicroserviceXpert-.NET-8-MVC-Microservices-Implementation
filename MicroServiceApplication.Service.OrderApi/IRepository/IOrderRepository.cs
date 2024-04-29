using MicroServiceApplication.Service.OrderApi.Const;
using MicroServiceApplication.Service.OrderApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.OrderApi.IRepository
{
    public interface IOrderRepository
    {
        ResponseDto AddOrder(CartDto cartDto);
        ResponseDto GetOrderForUser (string userId);
        ResponseDto CreateStripeSession(StripeRequestDto stripeRequestDto);
        ResponseDto ValidateSession(int orderHeaderId);
        ResponseDto GetAllOrder();
        ResponseDto GetOrderById(int Id);
        ResponseDto UpdateOrderStatus(int Id, string newStatus);



    }
}
