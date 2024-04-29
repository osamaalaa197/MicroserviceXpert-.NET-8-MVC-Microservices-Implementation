using MicroServiceApplication.Service.OrderApi.Dto;

namespace MicroServiceApplication.Service.OrderApi.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> DeleteUserCart(string userId);

    }
}
