using WholeSaler.Api.DTOs.Category;

namespace WholeSaler.Api.DTOs.ProductDTOs.EmbedDTOs
{
    public class ProductCategoryEmbedDTO
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public SubCategoryDTo SubCategory { get; set; }
        
    }
}
