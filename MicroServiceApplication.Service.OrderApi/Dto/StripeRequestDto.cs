﻿namespace MicroServiceApplication.Service.OrderApi.Dto
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl {  get; set; }
        public string? StripeSessionId { get; set; }
        public string? ApprovedUrl {  get; set; }
        public string? CanceledUrl { get; set; }
        public OrderHeaderDto OrderHeaderDto { get; set; }
    }
}
