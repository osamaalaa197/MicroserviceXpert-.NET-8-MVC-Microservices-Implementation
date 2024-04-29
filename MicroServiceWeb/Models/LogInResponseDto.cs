namespace MicroServiceWeb
{
    public class LogInResponseDto
    {
        public DateTime ExpireDate { get; set; }
        public string Token { get; set; }   
        public string UserId { get; set; }
    }
}
