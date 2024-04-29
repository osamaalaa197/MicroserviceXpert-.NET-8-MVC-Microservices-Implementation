namespace MicroServiceWeb
{
    public class OrderDetailsDto
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
        public string? ProductName { get; set; } = null;
        public double Price { get; set; }  
    }
}
