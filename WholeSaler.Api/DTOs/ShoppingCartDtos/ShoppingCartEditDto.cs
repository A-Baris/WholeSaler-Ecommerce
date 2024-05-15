using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.DTOs.ShoppingCartDtos
{
    public class ShoppingCartEditDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}
