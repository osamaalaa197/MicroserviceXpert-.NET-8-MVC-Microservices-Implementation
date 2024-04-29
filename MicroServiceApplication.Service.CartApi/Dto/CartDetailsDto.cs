﻿using MicroServiceApplication.Service.CartApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceApplication.Service.CartApi.Dto
{
	public class CartDetailsDto
	{
		public int Id { get; set; }
		public int CartHeaderId { get; set; }
		public CartHeaderDto? CartHeaderDto { get; set; }
		public int ProductId { get; set; }
		public ProductDto? ProductDto { get; set; }
		public int Count { get; set; }
	}
}