using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Comprehensive
{
    public class ProductComprehensiveVM
    {


        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public List<ProductImage>? Images { get; set; }
        public string? Description { get; set; }
        public ProductCategoryVM? Category { get; set; }
        public ProductStoreVM? Store { get; set; }
        public TelevisionVm? Television { get; set; }
        public LaptopVM? Laptop { get; set; }
        public PerfumeVM?  Perfume { get; set; }

    }
}
