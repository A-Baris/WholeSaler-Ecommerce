namespace WholeSaler.Web.Models.ViewModels.OrderVM
{
    public class OrderCreateVM
    {
        public string UserId { get; set; }
        public string ShoppingCartId { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public OrderShippingAddressVM ShippingAddress { get; set; }
        public OrderPaymentVM OrderPayment { get; set; }
    }
}
