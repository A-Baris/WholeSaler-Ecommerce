using WholeSaler.Web.Models.ViewModels.Product.Custom;

namespace WholeSaler.Web.Models.ViewModels.Product
{
    public class ProductGeneralVm
    {

        public List<TelevisionCustomVm> Televisions { get; set; } = new List<TelevisionCustomVm>();
        public List<LaptopCustomVm> Laptops { get; set; } = new List<LaptopCustomVm>();
        public List<PerfumeCustomVm> Perfumes { get; set; } = new List<PerfumeCustomVm>();
    }
}
