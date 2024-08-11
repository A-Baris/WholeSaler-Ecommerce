using WholeSaler.Entity.Entities.Products;

namespace WholeSaler.Api.DTOs.ProductDTOs.FilterProducts
{
    public class ProductFilterDto
    {
        public List<Television> Televisions { get; set; } = new List<Television>();
        public List<Laptop> Laptops { get; set; } = new List<Laptop>();
        public List<Perfume> Perfumes { get; set; } = new List<Perfume>();
    }
}
