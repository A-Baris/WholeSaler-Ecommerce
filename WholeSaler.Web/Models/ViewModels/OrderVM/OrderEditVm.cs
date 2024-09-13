﻿using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Models.ViewModels.OrderVM
{
    public class OrderEditVm
    {
        public string Id { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? ShoppingCartId { get; set; }
        public decimal? TotalOrderAmount { get; set; }
        public List<ProductForCartVM>? Products { get; set; }
        public OrderShippingAddressVM? ShippingAddress { get; set; }
        public OrderPaymentVM? OrderPayment { get; set; }
        public OrderCancellationRequestVm? CancellationRequest { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
