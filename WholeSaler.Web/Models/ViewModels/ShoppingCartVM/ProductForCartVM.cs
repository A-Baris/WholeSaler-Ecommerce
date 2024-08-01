using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;

namespace WholeSaler.Web.Models.ViewModels.ShoppingCartVM
{
    public class ProductForCartVM
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public List<ProductImage>? Images { get; set; }
        public CategoryForCartVM? Category { get; set; }
        public StoreForCartVM? Store { get; set; }
    }
}
