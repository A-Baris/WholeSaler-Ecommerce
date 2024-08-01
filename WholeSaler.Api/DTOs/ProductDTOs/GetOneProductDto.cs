using WholeSaler.Entity.Entities.Embeds.Product;
using WholeSaler.Entity.Entities.Products;

namespace WholeSaler.Api.DTOs.ProductDTOs
{
    public class GetOneProductDto
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public decimal Quantity { get; set; }
        public List<Comment>? Comments { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public string? Description { get; set; }
        public List<ProductImage>? Images { get; set; }
        public ProductCategory? Category { get; set; }
        public ProductStore? Store { get; set; }
        public Laptop? Laptop { get; set; }

        public Television? Television { get; set; }

        public Perfume? Perfume { get; set; }
    }
}
