using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WholeSaler.Web.Models.ViewModels.Attributes;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct
{
    public class ProductVm
    {
        [HiddenInput(DisplayValue = true)]
        public string? Id { get; set; }
        [DisplayOrder(1)]
        [DisplayName("Name")]
        
        public string? Name { get; set; }
        [HiddenInput]
        public string? Type { get; set; }
        [DisplayOrder(3)]
        [DisplayName("Color")]
        public string? Color { get; set; }
        [DisplayOrder(2)]
        [DisplayName("Brand")]
        public string? Brand { get; set; }
        [HiddenInput]
        public decimal Quantity { get; set; }
        [DisplayOrder(4)]
        [DisplayName("Unit Price")]
        public decimal? UnitPrice { get; set; }
        [DisplayOrder(5)]
        [DisplayName("Stock")]
        public decimal? Stock { get; set; }
        [HiddenInput(DisplayValue =true)]
        public int SumOfSales { get; set; }
        [DisplayOrder(6)]
        [DisplayName("Description")]
        public string? Description { get; set; }
        [HiddenInput]
        public List<ProductImage>? Images { get; set; }
        [HiddenInput(DisplayValue =true)]
        public ProductCategoryVM? Category { get; set; }
        [HiddenInput(DisplayValue = true)]
        public ProductStoreVM? Store { get; set; }
    }
}
