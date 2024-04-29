using MicroServiceApplication.Service.ProductAPI.Const;
using MicroServiceApplication.Service.ProductAPI.Dto;
using MicroServiceApplication.Service.ProductAPI.Repository;
using MicroServiceApplication.Service.ProductAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProudctRepository _proudctRepository;

        public ProductController(IProudctRepository proudctRepository)
        {
            _proudctRepository=proudctRepository;
        }
        [HttpGet]
        public ResponseDto GetAllProduct()
        {
            var res= _proudctRepository.GetAllProduct();
            return res;
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto DeleteProduct(int id)
        {
            var res = _proudctRepository.DeleteProduct(id);
            return res;
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ResponseDto GetProductById(int id)
        {
            var res = _proudctRepository.GetProductById(id);
            return res;
        }
        [HttpPut]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto UpdateProduct([FromForm]ProductDto productDto)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
            var res = _proudctRepository.UpdateProduct(productDto,baseUrl);
            return res;
        }
        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public ResponseDto AddProduct([FromForm]ProductDto productDto)
        {
            try
            {
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
				var res = _proudctRepository.AddProduct(productDto,baseUrl);
				return res;
			}
			catch (Exception ex) 
            { 
                var response=new ResponseDto()
                {
                    Message= ex.Message,    
                };
                return response;
            }
        }
    }
}
