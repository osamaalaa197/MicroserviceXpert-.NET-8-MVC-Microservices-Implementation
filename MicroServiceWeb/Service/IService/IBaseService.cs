using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Models;

namespace MicroServiceWeb.Service.IService
{
    public interface IBaseService
    {
        Task <ResponseDto> SendAsync(RequestDto requestDto,bool WithBearar=true);
    }
}
