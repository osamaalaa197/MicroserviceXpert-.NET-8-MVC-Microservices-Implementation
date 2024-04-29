namespace MicroServiceWeb.Const
{
    public class SD
    {
        public static string CouponApiBase { get; set; }
		public static string UserApiBase { get; set; }
        public static string ProductApiBase { get; set; }
        public static string CartApi {  get; set; }
        public static string OrderApi {  get; set; }

        public const string AdminRole = "Admin";
        public const string Customer = "Customer";
        public static string JwtToken { get; set; } = "JwtToken";

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickUp = "ReadyForPickUp";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
        public enum ContentType
        {
            Json,
            MultipartFormData
        }
    }
}
