using AutoMapper;
using MicroServiceApplication.Service.ProductAPI.Data;
using MicroServiceApplication.Service.ProductAPI.Dto;
using MicroServiceApplication.Service.ProductAPI.Models;
using MicroServiceApplication.Service.ProductAPI.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceApplication.Service.ProductAPI.Repository
{
    public class ProudctRepository : IProudctRepository
    {
        private readonly ProductContext _ProductContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProudctRepository(ProductContext productContext,IMapper mapper,IWebHostEnvironment webHostEnvironment)
        {
            _ProductContext=productContext; 
            _mapper=mapper;
            _webHostEnvironment=webHostEnvironment;
        }

        public ResponseDto AddProduct(ProductDto ProductDto, string baseurl)
        {
            var response=new ResponseDto();
            try
            {
                var product=_mapper.Map<Product>(ProductDto);
                _ProductContext.Products.Add(product);
                _ProductContext.SaveChanges();
                if (ProductDto.Image is not null)
                {
                    string fileName =product.Id+Path.GetExtension(ProductDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        ProductDto.Image.CopyTo(fileStream);
                    }
                    var imageUrl = baseurl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                    product.ImageUrl = imageUrl;
                }
                _ProductContext.SaveChanges();
                response.Result = product;
                response.IsSuccess=true;
                response.Message = "Product Add Successfully";
            }catch(Exception ex) 
            {
                response.IsSuccess = false;
                response.Message=ex.Message;
            }
            return response;
        }

        public ResponseDto DeleteProduct(int productId)
        {
            var response = new ResponseDto();
            try
            {
                var product = _ProductContext.Products.First(e => e.Id == productId);
                _ProductContext.Products.Remove(product);
                _ProductContext.SaveChanges();
                response.Result = product;
                response.IsSuccess = true;
                response.Message = "Product Removed Successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto GetAllProduct()
        {
            var response = new ResponseDto();
            try
            {
                var product = _ProductContext.Products.ToList();
                response.Result = product;
                response.IsSuccess = true;
                response.Message = "";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto GetProductById(int productId)
        {
            var response = new ResponseDto();
            try
            {
                var product= _ProductContext.Products.First(e=>e.Id==productId);
                response.Result = product;
                response.IsSuccess = true;
                response.Message = "";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto UpdateProduct(ProductDto productDto,string baseUrl)
        {
            var response = new ResponseDto();
            try
            {
                var product = _mapper.Map<Product>(productDto);
                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = product.Id + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                _ProductContext.Products.Update(product);
                _ProductContext.SaveChanges();
                response.Result = product;
                response.IsSuccess = true;
                response.Message = "Product Updated Successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
