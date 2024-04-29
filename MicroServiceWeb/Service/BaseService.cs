using MicroServiceApplication.CouponAPI.Dto;
using MicroServiceWeb.Models;
using MicroServiceWeb.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using static MicroServiceWeb.Const.SD;

namespace MicroServiceWeb.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory,ITokenProvider tokenProvider) 
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider= tokenProvider;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool WithBearar)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MicroServiceApi");
                var message = new HttpRequestMessage();
                if (requestDto.ContentType==ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");

                }
                else
                {
                    message.Headers.Add("Accept", "application/json");

                }
                //Token
                if (WithBearar) 
                {
                    var token= _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (requestDto.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                    }
                }

                HttpResponseMessage? apiResponse = null;
                switch (requestDto.ApiType)
                {
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDto { IsSuccess = false, Message = "User Unauthorized" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDto { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.NotFound:
                        return new ResponseDto { IsSuccess = false, Message = "Not Found" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return responseDto;
                }
            }
            catch (Exception ex)
            {
                var dto=new ResponseDto { IsSuccess = false,Message = ex.Message.ToString()};
                return dto;
            }
           
        }
    }
}
