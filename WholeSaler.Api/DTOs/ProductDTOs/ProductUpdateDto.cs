using WholeSaler.Entity.Entities.Embeds.Product;
using WholeSaler.Entity.Entities.Products;

namespace WholeSaler.Api.DTOs.ProductDTOs
{
    public class ProductUpdateDto
    {
        public string? Type { get; set; }
        public Laptop? Laptop { get; set; }

        public Television? Television { get; set; }

        public Perfume? Perfume { get; set; }
    }
}
