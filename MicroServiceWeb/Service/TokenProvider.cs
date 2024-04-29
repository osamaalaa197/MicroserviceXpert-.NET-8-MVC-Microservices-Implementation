using MicroServiceWeb.Const;
using MicroServiceWeb.Service.IService;
using Newtonsoft.Json.Linq;

namespace MicroServiceWeb.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor) 
        {
            _contextAccessor=contextAccessor;
        }
        public void CleanToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.JwtToken);
        }

        public string GetToken()
        {
            string? token = null;
            bool? hasToken= _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.JwtToken,out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.JwtToken, token);
        }
    }
}
