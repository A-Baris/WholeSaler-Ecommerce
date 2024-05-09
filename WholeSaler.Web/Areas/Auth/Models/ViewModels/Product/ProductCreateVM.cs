namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductCreateVM
    {
        public string? Name { get; set; }
        public string? Colour { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public string? Description { get; set; }
        public List<ProductImage>? Images { get; set; }
        public ProductCategoryVM? Category { get; set; }
        public ProductStoreVM? Store { get; set; }
    }
}
