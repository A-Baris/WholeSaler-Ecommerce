using WholeSaler.Api.DTOs.ProductDTOs.EmbedDTOs;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Embeds.Product;

namespace WholeSaler.Api.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Colour { get; set; }
        public decimal? Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Stock { get; set; }
        public string? Description { get; set; }
        public List<ProductImage> Images { get; set; }

        public ProductCategoryEmbedDTO Category { get; set; }
        public ProductStoreEmbedDTO Store { get; set; }
    }
}
