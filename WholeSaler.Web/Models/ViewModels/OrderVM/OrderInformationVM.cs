using System.ComponentModel;
using WholeSaler.Web.Models.Enums;

namespace WholeSaler.Web.Models.ViewModels.OrderVM
{
    public class OrderInformationVM
    {
        public string? Id { get; set; }
        [DisplayName("Order Date")]
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public EntityStatus Status { get; set; }
        public string ShoppingCartId { get; set; }
        [DisplayName("Total Price")]
        public decimal TotalOrderAmount { get; set; }
        public OrderShippingAddressVM ShippingAddress { get; set; }
        public OrderPaymentVM OrderPayment { get; set; }
    }
}
