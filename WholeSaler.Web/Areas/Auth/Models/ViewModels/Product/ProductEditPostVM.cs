using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.UpdateVMs;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductEditPostVM
    {
     
        public LaptopUpdateVm? Laptop { get; set; }
        public TelevisionUpdateVm? Television { get; set; }
        public PerfumeUpdateVm? Perfume { get; set; }
    }
}
