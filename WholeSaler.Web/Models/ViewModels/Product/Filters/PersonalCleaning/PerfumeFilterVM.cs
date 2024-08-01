namespace WholeSaler.Web.Models.ViewModels.Product.Filters.PersonalCleaning
{
    public class PerfumeFilterVM:BaseProductFilterVM
    {
        public string? Type { get; set; }
        public string? Smell { get; set; }
        public int? Volume { get; set; }
    }
}
