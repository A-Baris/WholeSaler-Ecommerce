namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Store
{
    public class SalesReportVM
    {
        public string Id { get; set; }
        public string StoreId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
