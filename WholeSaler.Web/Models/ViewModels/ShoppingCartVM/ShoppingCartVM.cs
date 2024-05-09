namespace WholeSaler.Web.Models.ViewModels.ShoppingCartVM
{
    public class ShoppingCartVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<ProductForCartVM>? Products { get; set; }
    }
}
