namespace WholeSaler.Web.Models.ViewModels.OrderVM
{
    public class OrderPaymentVM
    {
        public string CardOwnerName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }
    }
}
