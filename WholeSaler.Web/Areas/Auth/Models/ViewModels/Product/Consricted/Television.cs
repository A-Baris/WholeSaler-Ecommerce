using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted
{
    public class Television:ProductVm
    {
        public string? Inc { get; set; }
        public string? OperatingSystem { get; set; }
        public string? DisplayTech { get; set; }
        public string? Resolution { get; set; }
        public int? RefreshRate { get; set; }
        public int? YearOfProduction { get; set; }
        public bool? SateliteReceiver { get; set; }
        public bool? SmartTv { get; set; }
        public bool? Wifi { get; set; }
        public bool? Hdr { get; set; }
    }
}
