using WholeSaler.Web.Models.ViewModels.Product;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class TestVM
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public List<ProductCommentVM>? Comments { get; set; }
        public List<ProductImage>? Images { get; set; }
        public string? Description { get; set; }
        public ProductCategoryVM? Category { get; set; }
        public ProductStoreVM? Store { get; set; }
        public Dictionary<string, object> SpecialProperties { get; set; } = new Dictionary<string, object>();
    }
}
