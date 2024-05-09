using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Models.Enums;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Store
{
    public class MyStoreVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ProductImage>? Images { get; set; }
        public EntityStatus? Status { get; set; }
        public string Colour { get; set; }
        public decimal? Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int SumofSales { get; set; }
        public decimal? Stock { get; set; }
        public ProductCategoryVM  Category { get; set; }
        public ProductStoreVM  Store { get; set; }
    }
}
