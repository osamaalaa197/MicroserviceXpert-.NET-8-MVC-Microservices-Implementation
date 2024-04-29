using MicroServiceApplication.Service.CartApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.CartApi.Repository.IRepository
{
    public interface ICartRepository
    {
        ResponseDto CartUpsert(CartDto cartDto);
        Task<ResponseDto> GetCartForUser(string userId);
        ResponseDto RemoveCartDetails(int cartDetailId);
		Task<object> ApplyCoupon(CartDto cartDto);
        Task<object> RemoveCoupon(CartDto cartDto);
        ResponseDto DeleteCartForUser(string userId);


    }
}
