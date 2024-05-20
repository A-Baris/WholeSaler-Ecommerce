using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.DTOs.ShoppingCartDtos
{
    public class ShoppingCartDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Product>? Products { get; set; }
    }
}
