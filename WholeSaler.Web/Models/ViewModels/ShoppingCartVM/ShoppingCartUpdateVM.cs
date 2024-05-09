namespace WholeSaler.Web.Models.ViewModels.ShoppingCartVM
{
    public class ShoppingCartUpdateVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<ProductForCartVM>? Products { get; set; }
    }
}
