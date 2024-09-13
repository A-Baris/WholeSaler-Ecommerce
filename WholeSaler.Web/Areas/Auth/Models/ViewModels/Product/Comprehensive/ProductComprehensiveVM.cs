using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Comprehensive
{
    public class ProductComprehensiveVM
    {
        public string? Type { get; set; }
        public List<ProductImage>? Images { get; set; }
        public ProductCategoryVM? Category { get; set; }
        public ProductStoreVM? Store { get; set; }
        public Television? Television { get; set; }
        public Laptop? Laptop { get; set; }
        public Perfume?  Perfume { get; set; }

    }
}
