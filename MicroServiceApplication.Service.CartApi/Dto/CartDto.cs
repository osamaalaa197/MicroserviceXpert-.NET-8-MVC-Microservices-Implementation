﻿namespace MicroServiceApplication.Service.CartApi.Dto
{
	public class CartDto
	{
		public CartHeaderDto CartHeaderDto { get; set; }
		public IEnumerable<CartDetailsDto>? CartDetailsDto { get; set;}
	}
}
