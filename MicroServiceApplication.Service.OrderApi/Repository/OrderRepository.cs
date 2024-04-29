using AutoMapper;
using MicroServiceApplication.Service.OrderApi.Const;
using MicroServiceApplication.Service.OrderApi.Data;
using MicroServiceApplication.Service.OrderApi.Dto;
using MicroServiceApplication.Service.OrderApi.IRepository;
using MicroServiceApplication.Service.OrderApi.Models;
using MicroServiceApplication.Service.OrderApi.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace MicroServiceApplication.Service.OrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _Context;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public OrderRepository(OrderDbContext orderDbContext, IMapper mapper,IProductService productService,ICartService cartService)
        {
            _Context = orderDbContext;
            _mapper = mapper;
            _productService=productService;
            _cartService=cartService;
        }

        public ResponseDto AddOrder(CartDto cartDto)
        {
            var response = new ResponseDto();
            try
            {
                var orderHeader = _mapper.Map<OrderHeaderDto>(cartDto.CartHeaderDto);
                orderHeader.Status = SD.Status_Pending;
                orderHeader.OrderTime = DateTime.Now;
                orderHeader.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetailsDto);
                var orderCreated = new OrderHeader();
                orderCreated = _mapper.Map<OrderHeader>(orderHeader);
                orderCreated.OrderDetails = _mapper.Map<IEnumerable<OrderDetails>>(orderHeader.OrderDetails);
                _Context.OrderHeaders.Add(orderCreated);
                _Context.SaveChanges();
                orderHeader.OrderHeaderId = orderCreated.Id;
                response.Result = orderHeader;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            var response = new ResponseDto();
            try
            {
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CanceledUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };
                var discount = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions()
                    {
                        Coupon=stripeRequestDto.OrderHeaderDto.CouponCode
                    }
                };
                if (stripeRequestDto.OrderHeaderDto.Discount > 0)
                {
                    options.Discounts = discount;
                }
                foreach (var item in stripeRequestDto.OrderHeaderDto.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = (long)(item.ProductDto.Price),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions()
                            {
                                Name = item.ProductDto.Name,
                            },
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                var service = new Stripe.Checkout.SessionService();
                var session = service.Create(options);
                stripeRequestDto.StripeSessionId = session.Id;
                stripeRequestDto.StripeSessionUrl = session.Url;
                var orderHeader = _Context.OrderHeaders.First(e => e.Id == stripeRequestDto.OrderHeaderDto.OrderHeaderId);
                orderHeader.StripeSessiontId = session.Id;
                _Context.SaveChanges();
                response.IsSuccess = true;
                response.Result = stripeRequestDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


        public ResponseDto ValidateSession([FromBody] int orderHeaderId)
        {
            var response = new ResponseDto();
            try
            {
                var orderheader = _Context.OrderHeaders.Where(e => e.Id == orderHeaderId).Include(e => e.OrderDetails).FirstOrDefault();

                var service = new SessionService();
                var session = service.Get(orderheader.StripeSessiontId);

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);
                if (paymentIntent.Status == "succeeded")
                {
                    orderheader.PaymentIntentId = paymentIntent.Id;
                    orderheader.Status = SD.Status_Approved;
                    _Context.SaveChanges();
                    response.IsSuccess = true;
                    response.Result = _mapper.Map<OrderHeaderDto>(orderheader);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public ResponseDto GetOrderForUser(string userId)
        {
            var response = new ResponseDto();
            try
            {
                IEnumerable<OrderHeader> orderHeaders;
                orderHeaders = _Context.OrderHeaders.Include(e => e.OrderDetails).Where(e=>e.UserId==userId).OrderByDescending(e => e.Id);
                response.IsSuccess = true;
                response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(orderHeaders);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto GetAllOrder()
        {
            var response = new ResponseDto();
            try
            {
                IEnumerable<OrderHeader> orderHeaders;
                orderHeaders = _Context.OrderHeaders.Include(e => e.OrderDetails).OrderByDescending(e=>e.Id);

                response.IsSuccess = true;
                response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(orderHeaders);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto GetOrderById(int Id)
        {
            var response = new ResponseDto();
            try
            {
                var orderHeaders = _Context.OrderHeaders.Include(e => e.OrderDetails).First(e => e.Id == Id);
                var productDto = _productService.GetAllProduct().GetAwaiter().GetResult();
                var orderHeaderDto = _mapper.Map<OrderHeaderDto>(orderHeaders);
                foreach (var item in orderHeaderDto.OrderDetails)
                {
                    item.ProductDto = productDto.First(e => e.Id == item.ProductId);
                }
                response.IsSuccess = true;
                response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseDto UpdateOrderStatus(int Id,string newStatus)
        {
            var response = new ResponseDto();
            try
            {
                var orderHeaders = _Context.OrderHeaders.Include(e => e.OrderDetails).First(e => e.Id == Id);
                if (newStatus==SD.Status_Cancelled)
                {
                    var option = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeaders.PaymentIntentId,

                    };
                    var refundService = new RefundService();
                    var refund = refundService.Create(option);
                    orderHeaders.Status = newStatus;
                    _Context.SaveChanges();
                }
                orderHeaders.Status=newStatus;
                _Context.SaveChanges();
                response.IsSuccess = true;
                response.Result = _mapper.Map<OrderHeaderDto>(orderHeaders);
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
