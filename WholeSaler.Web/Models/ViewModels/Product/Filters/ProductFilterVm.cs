using System.Reflection;
using WholeSaler.Web.Models.ViewModels.Product.Custom;

namespace WholeSaler.Web.Models.ViewModels.Product.Filters
{
    public class ProductFilterVm
    {
        public TelevisionCustomVm Television { get; set; }
        public LaptopCustomVm Laptop { get; set; }
        public PerfumeCustomVm Perfume { get; set; }
      
    }
}
