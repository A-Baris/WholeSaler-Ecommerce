using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.DTOs.ProductDTOs
{
    public class ProductCreateTestDto
    {
        public ProductTest  ProductTest { get; set; }
        public FeatureDto Features { get; set; }
    }
}
