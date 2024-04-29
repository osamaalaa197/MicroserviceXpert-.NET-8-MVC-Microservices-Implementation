using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MicroServiceWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService= productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct() 
        { 
            var responseDto=await _productService.GetAllProduct();
            List<ProductDto> data = new () ;
            if (responseDto != null && responseDto.IsSuccess) 
            {
                data = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
                return View(data);
            }
            else
            {
                TempData["error"] = responseDto?.Message;

            }
            return View(data);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var productDto = new ProductDto()
			{
				Header = "Create Product",
				Action = "SaveProduct",
				ButtonTitle = "Create"

			};
			return View("ProductForm", productDto);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ProductForm", productDto);

            }
            var responseDto = await _productService.AddProduct(productDto);
            if (responseDto.Result != null && responseDto.IsSuccess)
            {
                TempData["success"] = responseDto?.Message;
                return RedirectToAction(nameof(GetAllProduct));
            }
            else
            {
                TempData["error"] = responseDto?.Message;

            }
            return View("ProductForm", productDto);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
			var responseDto = await _productService.GetProducById(productId);
			if (responseDto != null && responseDto.IsSuccess)
			{
				var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
				productDto.Header = "Delete Product";
				productDto.Action = "RemoveProduct";
				productDto.ButtonTitle = "Delete";
				return View("ProductForm", productDto);
			}
			TempData["error"] = responseDto.Message;
			return NotFound();
		}
        [HttpPost]
        public async Task<IActionResult> RemoveProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.DeleteProduct(productDto.Id);
                if (res != null && res.IsSuccess)
                {
                    TempData["success"] = res.Message;
                    return RedirectToAction("GetAllProduct");
                }
				TempData["error"] = res.Message;
			}
			return View("ProductForm", productDto);
		}
		[HttpGet]
		public async Task<IActionResult> UpdateProduct(int productId)
		{
			var responseDto = await _productService.GetProducById(productId);
			if (responseDto != null && responseDto.IsSuccess)
			{
				var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
				productDto.Header = "Update Product";
				productDto.Action = "UpdateProduct";
				productDto.ButtonTitle = "Save";
                productDto.Id= productId;
				return View("ProductForm", productDto);
			}
			TempData["error"] = responseDto.Message;
			return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateProduct(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				var res = await _productService.UpdateProduct(productDto);
				if (res != null && res.IsSuccess)
				{
					TempData["success"] = res.Message;
					return RedirectToAction("GetAllProduct");
				}
				TempData["error"] = res.Message;
			}
			return View("ProductForm", productDto);
		}
	}
}
