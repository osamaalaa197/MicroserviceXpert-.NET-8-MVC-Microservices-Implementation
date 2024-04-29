using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceApplication.Service.CartApi.Models
{
	public class CartHeader
	{
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string? CouponCode { get; set; }
		[NotMapped]
		public int Discount { get; set; }
		[NotMapped]
		public int CartTotal { get; set; }
	}
}
