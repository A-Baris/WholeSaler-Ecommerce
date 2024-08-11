using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.UpdateVMs;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductEditVM
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public List<ProductImage>? Images { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int? SumOfSales { get; set; }
        public string? Description { get; set; }
        public ProductCategoryVM? Category { get; set; }
        public ProductStoreVM? Store { get; set; }
        public Laptop? Laptop { get; set; }
        public Consricted.Television? Television { get; set; }
        public Consricted.Perfume? Perfume { get; set; }
    }
}
