namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductVM
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<ProductImage>? Images { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Brand { get; set; }
        public int? Inc { get; set; }
        public string? Processor { get; set; }
        public int? RAM { get; set; }
        public int SumOfSales { get; set; }
        public string? Description { get; set; }
        public decimal? Stock { get; set; }
        public ProductCategoryVM? Category { get; set; }
        public ProductStoreVM? Store { get; set; }
    }
}
