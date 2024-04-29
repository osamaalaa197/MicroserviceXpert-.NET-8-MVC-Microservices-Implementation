namespace MicroServiceWeb.Service.IService
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string GetToken();
        void CleanToken();

    }
}
