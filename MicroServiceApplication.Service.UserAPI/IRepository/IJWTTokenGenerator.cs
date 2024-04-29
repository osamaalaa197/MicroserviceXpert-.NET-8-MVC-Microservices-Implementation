using MicroServiceApplication.Service.UserAPI.Models;

namespace MicroServiceApplication.Service.UserAPI.IRepository
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> roles);
    }
}
