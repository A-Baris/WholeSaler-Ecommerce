namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Store
{
    public class StoreCreateVM
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Adress Adress { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxNumber { get; set; }

        public string? UserId { get; set; }
     

    }
}
