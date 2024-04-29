using MicroServiceApplication.Service.OrderApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceApplication.Service.OrderApi.Dto
{
    public class OrderDetailsDto
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }  
    }
}
