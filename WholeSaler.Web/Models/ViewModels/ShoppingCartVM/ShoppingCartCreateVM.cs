namespace WholeSaler.Web.Models.ViewModels.ShoppingCartVM
{
    public class ShoppingCartCreateVM
    {

        public string? UserId { get; set; }
        public List<ProductForCartVM>? Products { get; set; }

    }
}
