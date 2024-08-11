using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.UpdateVMs
{
    public class Television
    {
        [HiddenInput]
        public string? Id { get; set; }
        [DisplayName("Name")]
        public string? Name { get; set; }
        public string? Type { get; set; }
        [DisplayName("Brand")]
        public string? Brand { get; set; }
        [DisplayName("Color")]
        public string? Color { get; set; }
        
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
        [HiddenInput]
        public ProductCategoryVM? Category { get; set; }
        [HiddenInput]
        public ProductStoreVM? Store { get; set; }
        [DisplayName("Inc")]
        public string? Inc { get; set; }
        [DisplayName("Operation System")]
        public string? Os { get; set; }
    }
}
