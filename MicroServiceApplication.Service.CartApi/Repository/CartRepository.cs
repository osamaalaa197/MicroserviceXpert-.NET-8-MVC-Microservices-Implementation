using AutoMapper;
using MicroServiceApplication.Service.CartApi.Data;
using MicroServiceApplication.Service.CartApi.Dto;
using MicroServiceApplication.Service.CartApi.Models;
using MicroServiceApplication.Service.CartApi.Repository.IRepository;
using MicroServiceApplication.Service.CartApi.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace MicroServiceApplication.Service.CartApi.Repository
{
	public class CartRepository:ICartRepository
	{
		private readonly CartDbContext _Context;
		private readonly IMapper _mapper;
		private readonly IProductService _productService;
		private readonly ICouponService _couponService;

		public CartRepository(CartDbContext cartDbContext,IMapper mapper,IProductService productService,ICouponService couponService) 
		{
			_Context= cartDbContext;
			_mapper= mapper;
			_productService= productService;
			_couponService= couponService;
		}

		public ResponseDto CartUpsert(CartDto cartDto)
		{
			var response= new ResponseDto();
			try 
			{
				var isHasCatHeader=_Context.CartHeaders.AsNoTracking().FirstOrDefault(e=>e.UserId==cartDto.CartHeaderDto.UserId);
                if (isHasCatHeader == null)
                {
					var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeaderDto);
					_Context.CartHeaders.Add(cartHeader);
					_Context.SaveChanges();
					cartDto.CartDetailsDto.First().CartHeaderId = cartHeader.Id;
					_Context.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetailsDto.First()));
					_Context.SaveChanges();
					response.IsSuccess = true;
					response.Message = "Product Add Successfully";
				}
				else
				{
					var CartDetailsFromDb=_Context.CartDetails.AsNoTracking().FirstOrDefault(e=>e.ProductId==cartDto.CartDetailsDto.First().ProductId 
					&& e.CartHeaderId==isHasCatHeader.Id);
					if(CartDetailsFromDb == null)
					{
						//createCartDetails
						cartDto.CartDetailsDto.First().CartHeaderId = isHasCatHeader.Id;
						_Context.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetailsDto.First()));
						_Context.SaveChanges();
					}
					else
					{
						//Update Count
						cartDto.CartDetailsDto.First().Count += CartDetailsFromDb.Count;
						cartDto.CartDetailsDto.First().CartHeaderId = CartDetailsFromDb.CartHeaderId;
						cartDto.CartDetailsDto.First().Id = CartDetailsFromDb.Id;
						_Context.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetailsDto.First()));
						_Context.SaveChanges();

					}
				}

               
			}
			catch(Exception ex) 
			{ 
				response.IsSuccess= false;
				response.Message= ex.Message;
			}
			return response;
			
		}
		public ResponseDto RemoveCartDetails([FromBody] int cartDetailId)
		{
			var response=new ResponseDto();
			try
			{
				var cartDetails = _Context.CartDetails.First(e => e.Id == cartDetailId);
				int countDetailiteminHeader=_Context.CartDetails.Where(e=>e.CartHeaderId==cartDetails.CartHeaderId).Count();
				_Context.CartDetails.Remove(cartDetails);
                if (countDetailiteminHeader==1)
                {
					var cartHeaderToRemove = _Context.CartHeaders.First(e => e.Id == cartDetails.CartHeaderId);
					_Context.CartHeaders.Remove(cartHeaderToRemove);
					_Context.SaveChanges();
                }
				_Context.SaveChanges();
				response.IsSuccess= true;
            }
            catch(Exception ex) 
			{ 
				response.Message = ex.Message;
			}
			return response;
		}
		public async Task<ResponseDto> GetCartForUser(string userId) 
		{
			var response= new ResponseDto();
			try
			{
				var cartDto=new CartDto();
				var cartHeader=_Context.CartHeaders.First(e=>e.UserId==userId);
				var cartDetails=_Context.CartDetails.Where(e=>e.CartHeaderId==cartHeader.Id).ToList();
				cartDto.CartHeaderDto=_mapper.Map<CartHeaderDto>(cartHeader);
				cartDto.CartDetailsDto = _mapper.Map<List<CartDetailsDto>>(cartDetails);
				var productDto = await _productService.GetAllProduct();
				foreach (var item in cartDto.CartDetailsDto) 
				{
					item.ProductDto=productDto.First(e=>e.Id==item.ProductId);
					cartDto.CartHeaderDto.CartTotal += (item.Count * item.ProductDto.Price);
				}
				response.IsSuccess= true;
				if(!string.IsNullOrEmpty(cartDto.CartHeaderDto.CouponCode)) 
				{
					CouponDto couponDto= await _couponService.GetCouponByCouponCode(cartDto.CartHeaderDto.CouponCode);
                    if (couponDto!=null && cartDto.CartHeaderDto.CartTotal>couponDto.MinAmount)
                    {
						cartDto.CartHeaderDto.CartTotal -= couponDto.DiscountAmount;
						cartDto.CartHeaderDto.Discount = couponDto.DiscountAmount;
                    }

                }
				response.Result= cartDto;
			}catch(Exception ex) 
			{
				response.Message = ex.Message;
				response.IsSuccess= false;
			}
			return response;
		}
		public async Task<object> ApplyCoupon(CartDto cartDto)
		{
			var response= new ResponseDto();
			try
			{
                var couponDto = await _couponService.GetCouponByCouponCode(cartDto.CartHeaderDto.CouponCode);
                if (couponDto.CouponId!=0)
				{
                    var cartheader = await _Context.CartHeaders.FirstAsync(e => e.UserId == cartDto.CartHeaderDto.UserId);
                    cartheader.CouponCode = cartDto.CartHeaderDto.CouponCode;
                    _Context.CartHeaders.Update(cartheader);
                    await _Context.SaveChangesAsync();
                    response.IsSuccess = true;

                }
                response.IsSuccess = false;
                response.Message = "Invalid Coupon";

            }
            catch(Exception ex) 
			{
				response.Message = ex.Message;
			}
			return response;
		}
		public async Task<object> RemoveCoupon(CartDto cartDto)
		{
			var response = new ResponseDto();
			try
			{
				var cartheader = await _Context.CartHeaders.FirstAsync(e => e.UserId == cartDto.CartHeaderDto.UserId);
				cartheader.CouponCode = "";
				_Context.CartHeaders.Update(cartheader);
				await _Context.SaveChangesAsync();
				response.IsSuccess = true;
			}
			catch (Exception ex)
			{
				response.IsSuccess=false;
				response.Message = ex.Message;
			}
			return response;
		}

        public ResponseDto DeleteCartForUser(string userId)
        {
			var response = new ResponseDto();
			try
			{
				var carHeader = _Context.CartHeaders.First(e => e.UserId == userId);
				var cartDetails=_Context.CartDetails.Where(e=>e.CartHeaderId == carHeader.Id).ToList();
                _Context.CartDetails.RemoveRange(cartDetails);
				_Context.SaveChanges();
                _Context.CartHeaders.Remove(carHeader);
                _Context.SaveChanges();

            }
            catch (Exception ex) 
			{
				response.IsSuccess = false;
				response.Message= ex.Message;
			}
			return response;
        }
    }
}
