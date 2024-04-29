using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceWeb
{
	public class CartHeaderDto
	{
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string? CouponCode { get; set; }
		public double Discount { get; set; }
		public double CartTotal { get; set; }
		[Required]
		public string? Name { get; set; }
        [Required]
		[Phone]
        public string? Phone { get; set; }
        [Required]
		[EmailAddress]
        public string? Email { get; set; }
	}
}
