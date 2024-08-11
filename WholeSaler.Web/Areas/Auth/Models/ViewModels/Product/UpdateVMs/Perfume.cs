using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.UpdateVMs
{
    public class Perfume
    {
        [HiddenInput(DisplayValue = true)]
        public string? Id { get; set; }
        [DisplayName("Name")]
        public string? Name { get; set; }
        public string? Type { get; set; }
        [DisplayName("Brand")]
        public string? Brand { get; set; }
        [DisplayName("Color")]
        public string? Color { get; set; }
        [HiddenInput]
        public List<ProductImage>? Images { get; set; }
        [HiddenInput]
        public decimal? Quantity { get; set; }
        [DisplayName("Unit Price")]
        public decimal? UnitPrice { get; set; }
        [DisplayName("Stock")]
        public decimal? Stock { get; set; }
        [HiddenInput]
        public int? SumOfSales { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }
        [HiddenInput(DisplayValue = true)]
        public ProductCategoryVM? Category { get; set; }
        [HiddenInput(DisplayValue = true)]
        public ProductStoreVM? Store { get; set; }
        public string? PerfumeType { get; set; }
        public string? Smell { get; set; }
        public int? Volume { get; set; }
    }
}
