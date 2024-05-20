using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Api.DTOs.Store
{
    public class StoreDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Adress? Adress { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxNumber { get; set; }
        public AdminConfirmation? AdminConfirmation { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? UserId { get; set; }
    }
}
