﻿namespace MicroServiceWeb
{
	public class CartDto
	{
		public CartHeaderDto CartHeaderDto { get; set; }
		public IEnumerable<CartDetailsDto>? CartDetailsDto { get; set;}
	}
}
