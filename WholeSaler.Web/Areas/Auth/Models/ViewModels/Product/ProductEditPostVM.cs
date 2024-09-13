

using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductEditPostVM
    {
        public string? Type { get; set; }
        public Laptop? Laptop { get; set; }
        public Television? Television { get; set; }
        public Perfume? Perfume { get; set; }
    }
}
