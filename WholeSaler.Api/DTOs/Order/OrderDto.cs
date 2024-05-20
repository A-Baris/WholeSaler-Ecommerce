using WholeSaler.Entity.Entities.Embeds.Order;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.DTOs.Order
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ShoppingCartId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Product>? Products { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public OrderPayment OrderPayment { get; set; }
    }
}
