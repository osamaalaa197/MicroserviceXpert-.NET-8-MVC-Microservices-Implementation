using MicroServiceApplication.Service.UserAPI.Models;

namespace MicroServiceApplication.Service.UserAPI.Dto
{
    public class LogInResponseDto
    {
        public string Token { get; set; }   
        public string UserId { get; set; }
    }
}
