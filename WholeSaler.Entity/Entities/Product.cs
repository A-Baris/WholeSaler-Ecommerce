using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Embeds.Product;

namespace WholeSaler.Entity.Entities
{
    public class Product:BaseEntity
    {
       

        public string? Name { get; set; }
        public string? Colour { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ProductImage>? Images { get; set; }
        public string? Description { get; set; }
        public ProductCategory?  Category { get; set; }
        public ProductStore?  Store { get; set; }



    }
}
