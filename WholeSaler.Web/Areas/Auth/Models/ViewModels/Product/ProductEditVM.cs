using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;


namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductEditVM
    {

        public string? Type { get; set; }
      
        public Laptop? Laptop { get; set; }
        public Television? Television { get; set; }
        public Perfume? Perfume { get; set; }
    }
}
