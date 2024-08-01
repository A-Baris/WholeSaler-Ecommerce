using WholeSaler.Web.Areas.Admin.Models.Enums;

namespace WholeSaler.Web.Areas.Admin.Models.ViewModels.Store
{
    public class StoreVM
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public AddresForStoreVM? Adress { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxNumber { get; set; }
        public AdminConfirmation? AdminConfirmation { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UserId { get; set; }
    }
}
