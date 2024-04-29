using System.ComponentModel.DataAnnotations;

namespace MicroServiceApplication.Service.OrderApi
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageLocalPath { get; set; }
		public string? Action { get; set; }
		public string? Header { get; set; }
		public string? ButtonTitle { get; set; }
        [Range(1,100)]
        public int Count { get; set; } = 1;
	}
}
