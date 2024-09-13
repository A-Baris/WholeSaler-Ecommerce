using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Embeds.Product;
using WholeSaler.Entity.Entities.Enums;
using WholeSaler.Entity.Entities.Products;
using WholeSaler.Entity.Entities.Products.Features;

namespace WholeSaler.Entity.Entities
{
    [BsonIgnoreExtraElements]
    public class Product : BaseEntity
    {


        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ProductImage>? Images { get; set; }
        public string? Description { get; set; }
        public ProductCategory? Category { get; set; }
        public ProductStore? Store { get; set; }
        public ProductOrderStatus? OrderStatus { get; set; }

    }
}
