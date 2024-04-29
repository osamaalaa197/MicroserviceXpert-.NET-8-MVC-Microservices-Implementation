using MicroServiceApplication.Service.CartApi.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceApplication.Service.CartApi.Models
{
	public class CartDetails
	{
		public int Id { get; set; }
		public int CartHeaderId { get; set; }
		public CartHeader CartHeader { get; set; }
		public int ProductId { get; set; }
		[NotMapped]
		public ProductDto ProductDto { get; set; }
		public int Count { get; set; }
	}
}
