using WholeSaler.Web.Models.Enums;

namespace WholeSaler.Web.Models.ViewModels.OrderVM
{
    public class OrderInformationVM
    {
        public string? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; }
        public EntityStatus Status { get; set; }
        public string ShoppingCartId { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public OrderShippingAddressVM ShippingAddress { get; set; }
        public OrderPaymentVM OrderPayment { get; set; }
    }
}
