using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServiceApplication.Service.OrderApi.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public OrderHeader? OrderHeader  { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
    }
}
