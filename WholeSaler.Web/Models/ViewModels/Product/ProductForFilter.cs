using System.Reflection;
using WholeSaler.Web.Models.ViewModels.Product.Filters.Electronics;
using WholeSaler.Web.Models.ViewModels.Product.Filters;

namespace WholeSaler.Web.Models.ViewModels.Product
{
    public class ProductForFilter
    {
        public BaseProductFilterVM BaseFilter { get; set; }
        public TelevisionFilterVM TelevisionFilter { get; set; }
        public LaptopFilterVM LaptopFilter { get; set; }

        public ProductForFilter()
        {
            BaseFilter = new BaseProductFilterVM();
            TelevisionFilter = new TelevisionFilterVM();
            LaptopFilter = new LaptopFilterVM();
        }

        public object GetActiveFilter()
        {
            if (TelevisionFilter != null) return TelevisionFilter;
            if (LaptopFilter != null) return LaptopFilter;
            return BaseFilter;
        }
    }
}
