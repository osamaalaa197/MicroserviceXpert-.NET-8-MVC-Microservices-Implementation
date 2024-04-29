using static MicroServiceWeb.Const.SD;

namespace MicroServiceWeb.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public object Data { get; set; }
        public string Url { get; set; }
        public string AccessToken { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
